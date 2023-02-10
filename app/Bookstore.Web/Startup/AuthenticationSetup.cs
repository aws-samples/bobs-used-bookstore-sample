using Bookstore.Domain.Customers;
using Bookstore.Web.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Bookstore.Web.Startup
{
    public static class CognitoClientIdHelper
    {
        public static string GetClientId(WebApplicationBuilder builder)
        {
            switch (builder.Configuration["AWS:Service"])
            {
                case "EC2":
                    return builder.Configuration["Authentication:Cognito:EC2ClientId"];

                case "AppRunner":
                    return builder.Configuration["Authentication:Cognito:AppRunnerClientId"];

                default:
                    return builder.Configuration["Authentication:Cognito:LocalClientId"];
            }
        }
    }

    public static class AuthenticationSetup
    {
        private static string _cognitoDomain;
        private static string _cognitoClientId;
        private static string _cognitoAppSignOutUrl;

        public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            _cognitoDomain = builder.Configuration["Authentication:Cognito:CognitoDomain"];
            _cognitoClientId = CognitoClientIdHelper.GetClientId(builder);
            _cognitoAppSignOutUrl = builder.Configuration["Authentication:Cognito:AppSignOutUrl"];

            // For the 'Development' profile we fake authentication. For 'Test' and 'Production'
            // profiles we use Amazon Cognito's Hosted UI
            return builder.Environment.IsDevelopment() ? ConfigureLocalAuthentication(builder) : ConfigureCognitoAuthentication(builder);
        }

        private static WebApplicationBuilder ConfigureLocalAuthentication(WebApplicationBuilder builder)
        {
            builder.Services
                .AddAuthentication(x =>
            {
                x.AddScheme<LocalAuthenticationHandler>("localauth", null);
                x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = "localauth";
            }).AddCookie();

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
                    x.ClientId = CognitoClientIdHelper.GetClientId(builder);
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "cognito:username",
                        RoleClaimType = "cognito:groups"
                    };

                    x.Events.OnRedirectToIdentityProviderForSignOut = OnRedirectToIdentityProviderForSignOut;
                    x.Events.OnTokenValidated = SaveCustomerDetailsAsync;
                });

            return builder;
        }

        private static Task OnRedirectToIdentityProviderForSignOut(RedirectContext context)
        {
            context.ProtocolMessage.Scope = "openid";
            context.ProtocolMessage.ResponseType = "code";

            var logoutUrl = $"{context.Request.Scheme}://{context.Request.Host}{_cognitoAppSignOutUrl}";

            context.ProtocolMessage.IssuerAddress = $"{_cognitoDomain}/logout?client_id={_cognitoClientId}&logout_uri={logoutUrl}";

            context.Properties.Items.Remove(CookieAuthenticationDefaults.AuthenticationScheme);

            context.Properties.Items.Remove(OpenIdConnectDefaults.AuthenticationScheme);

            return Task.CompletedTask;
        }

        private static async Task SaveCustomerDetailsAsync(TokenValidatedContext context)
        {
            var customerService = context.HttpContext.RequestServices.GetService<ICustomerService>();

            var dto = new CreateOrUpdateCustomerDto(
                context.Principal.GetSub(),
                context.Principal.Identity.Name,
                context.Principal.FindFirst("given_name").Value,
                context.Principal.FindFirst("family_name").Value);

            await customerService.CreateOrUpdateCustomerAsync(dto);
        }
    }
}