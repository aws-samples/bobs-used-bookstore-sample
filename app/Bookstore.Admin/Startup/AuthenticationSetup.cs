using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace AdminSite.Startup
{
    public static class AuthenticationSetup
    {
        private static string _cognitoDomain;
        private static string _cognitoClientId;
        private static string _cognitoAppSignOutUrl;

        public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            _cognitoDomain = builder.Configuration["Authentication:Cognito:CognitoDomain"];
            _cognitoClientId = builder.Configuration["Authentication:Cognito:ClientId"];
            _cognitoAppSignOutUrl = builder.Configuration["Authentication:Cognito:AppSignOutUrl"];

            return builder.Environment.IsDevelopment() ? ConfigureLocalAuthentication(builder) : ConfigureCognitoAuthentication(builder);
        }

        private static WebApplicationBuilder ConfigureLocalAuthentication(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(x =>
            {
                x.AddScheme<LocalAuthenticationHandler>("localauth", null);
                x.DefaultAuthenticateScheme = "localauth";
                x.DefaultChallengeScheme = "localauth";
                x.DefaultSignOutScheme = "localauth";
            });

            return builder;
        }

        private static WebApplicationBuilder ConfigureCognitoAuthentication(WebApplicationBuilder builder)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            builder.Services
                .AddAuthentication(x =>
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

                    x.Events.OnRedirectToIdentityProviderForSignOut = OnRedirectToIdentityProviderForSignOut;
                });

            return builder;
        }

        private static Task OnRedirectToIdentityProviderForSignOut(RedirectContext context)
        {
            context.ProtocolMessage.Scope = "openid";
            context.ProtocolMessage.ResponseType = "code";

            var logoutUrl = $"{context.Request.Scheme}://{context.Request.Host}{_cognitoAppSignOutUrl}";

            context.ProtocolMessage.IssuerAddress = $"{_cognitoDomain}/logout?client_id={_cognitoClientId}&logout_uri={logoutUrl}";

            // delete cookies
            context.Properties.Items.Remove(CookieAuthenticationDefaults.AuthenticationScheme);

            // close openid session
            context.Properties.Items.Remove(OpenIdConnectDefaults.AuthenticationScheme);

            return Task.CompletedTask;
        }
    }
}