using System;
using Bookstore.Domain.Customers;
using Bookstore.Web.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Bookstore.Web.Startup;

public static class AuthenticationSetup
{
    public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        return builder.Configuration["Services:Authentication"] == "aws" 
            ? ConfigureCognitoAuthentication(builder) 
            : ConfigureLocalAuthentication(builder);
    }

    private static WebApplicationBuilder ConfigureLocalAuthentication(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(x =>
        {
            x.AddScheme<LocalAuthenticationHandler>("LocalAuthentication", null);
            x.DefaultAuthenticateScheme = "LocalAuthentication";
            x.DefaultChallengeScheme = "LocalAuthentication";
        });
            
        return builder;
    }

    private static WebApplicationBuilder ConfigureCognitoAuthentication(WebApplicationBuilder builder)
    {
        Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

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
                x.ResponseType = OpenIdConnectResponseType.Code;
                x.MetadataAddress = builder.Configuration["Cognito:MetadataAddress"];
                x.ClientId = GetCognitoClientId(builder.Configuration);
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "cognito:username",
                    RoleClaimType = "cognito:groups"
                };

                x.Events.OnRedirectToIdentityProviderForSignOut = y => OnRedirectToIdentityProviderForSignOut(y, builder.Configuration);
                x.Events.OnTokenValidated = SaveCustomerDetailsAsync;
            });

        return builder;
    }

    private static string GetCognitoClientId(IConfiguration configuration)
    {
        var clientSsmParameterName = configuration["Cognito:ClientIdSSMParameterName"];

        Console.WriteLine("clientSsmParameterName: {0}", clientSsmParameterName);
        if (clientSsmParameterName == null) throw new ArgumentNullException("configuration[\"Cognito:ClientIdSSMParameterName\"]");

        Console.WriteLine("configuration[clientSsmParameterName]: {0}", configuration[clientSsmParameterName]);
        return configuration[clientSsmParameterName];
    }

    private static Task OnRedirectToIdentityProviderForSignOut(RedirectContext context, IConfiguration configuration)
    {
        context.ProtocolMessage.Scope = "openid";
        context.ProtocolMessage.ResponseType = "code";

        var logoutUrl = $"{context.Request.Scheme}://{context.Request.Host}/";

        context.ProtocolMessage.IssuerAddress = $"{configuration["Cognito:CognitoDomain"]}/logout?client_id={GetCognitoClientId(configuration)}&logout_uri={logoutUrl}";

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
            context.Principal.FindFirst("given_name")?.Value,
            context.Principal.FindFirst("family_name")?.Value);

        await customerService.CreateOrUpdateCustomerAsync(dto);
    }
}