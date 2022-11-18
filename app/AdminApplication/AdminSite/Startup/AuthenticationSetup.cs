using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdminSite.Startup
{
    public static class AuthenticationSetup
    {
        private static bool _isDevelopment;
        private static string _cognitoDomain;
        private static string _cognitoClientId;
        private static string _cognitoAppSignOutUrl;

        public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            _isDevelopment = builder.Environment.IsDevelopment();
            _cognitoDomain = builder.Configuration["Authentication:Cognito:CognitoDomain"];
            _cognitoClientId = builder.Configuration["Authentication:Cognito:ClientId"];
            _cognitoAppSignOutUrl = builder.Configuration["Authentication:Cognito:AppSignOutUrl"];

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // Configure Authentication
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(x =>
            {
                x.ResponseType = builder.Configuration["Authentication:Cognito:ResponseType"];
                x.MetadataAddress = builder.Configuration["Authentication:Cognito:MetadataAddress"];
                x.ClientId = builder.Configuration["Authentication:Cognito:ClientId"];
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "cognito:username"
                };

                x.Events.OnRedirectToIdentityProvider = OnRedirectToIdentityProvider;
                x.Events.OnRedirectToIdentityProviderForSignOut = OnRedirectToIdentityProviderForSignOut;
            });

            return builder;
        }

        private static async Task OnRedirectToIdentityProviderForSignOut(RedirectContext context)
        {
            if (_isDevelopment)
            {
                context.Response.Redirect("/");

                context.HandleResponse();

                await context.HttpContext.SignOutAsync();

                return;
            }

            context.ProtocolMessage.Scope = "openid";
            context.ProtocolMessage.ResponseType = "code";

            var logoutUrl = $"{context.Request.Scheme}://{context.Request.Host}{_cognitoAppSignOutUrl}";

            context.ProtocolMessage.IssuerAddress = $"{_cognitoDomain}/logout?client_id={_cognitoClientId}&logout_uri={logoutUrl}";

            // delete cookies
            context.Properties.Items.Remove(CookieAuthenticationDefaults.AuthenticationScheme);

            // close openid session
            context.Properties.Items.Remove(OpenIdConnectDefaults.AuthenticationScheme);
        }

        private static async Task OnRedirectToIdentityProvider(RedirectContext context)
        {
            if (!_isDevelopment) return;

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "admin"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "admin user"));

            context.Response.Redirect("/");

            context.HandleResponse();

            await context.HttpContext.SignInAsync(new ClaimsPrincipal(identity));
        }
    }
}
