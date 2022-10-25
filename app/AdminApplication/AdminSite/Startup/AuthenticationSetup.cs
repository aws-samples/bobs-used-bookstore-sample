using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdminSite.Startup
{
    public static class AuthenticationSetup
    {
        private static WebApplicationBuilder _builder;

        public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            _builder = builder;

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // Configure Authentication
            _builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(x =>
            {
                x.ResponseType = _builder.Configuration["Authentication:Cognito:ResponseType"];
                x.MetadataAddress = _builder.Configuration["Authentication:Cognito:MetadataAddress"];
                x.ClientId = _builder.Configuration["Authentication:Cognito:ClientId"];
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "cognito:username"
                };

                x.Events.OnRedirectToIdentityProvider = OnRedirectToIdentityProvider;
                x.Events.OnRedirectToIdentityProviderForSignOut = OnRedirectToIdentityProviderForSignOut;
            });

            return _builder;
        }

        private static Task OnRedirectToIdentityProviderForSignOut(RedirectContext context)
        {
            context.ProtocolMessage.Scope = "openid";
            context.ProtocolMessage.ResponseType = "code";

            var cognitoDomain = _builder.Configuration["Authentication:Cognito:CognitoDomain"];

            var clientId = _builder.Configuration["Authentication:Cognito:ClientId"];

            var logoutUrl = $"{context.Request.Scheme}://{context.Request.Host}{_builder.Configuration["Authentication:Cognito:AppSignOutUrl"]}";

            context.ProtocolMessage.IssuerAddress = $"{cognitoDomain}/logout?client_id={clientId}&logout_uri={logoutUrl}";

            // delete cookies
            context.Properties.Items.Remove(CookieAuthenticationDefaults.AuthenticationScheme);

            // close openid session
            context.Properties.Items.Remove(OpenIdConnectDefaults.AuthenticationScheme);

            return Task.CompletedTask;
        }

        private static async Task OnRedirectToIdentityProvider(RedirectContext context)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "admin"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "admin user"));

            context.Response.Redirect("/");

            context.HandleResponse();

            await context.HttpContext.SignInAsync(new ClaimsPrincipal(identity));
        }
    }
}
