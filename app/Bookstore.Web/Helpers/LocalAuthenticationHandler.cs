using Bookstore.Domain.Customers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Bookstore.Web.Helpers
{
    public class LocalAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string UserId = "FB6135C7-1464-4A72-B74E-4B63D343DD09";

        public LocalAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(ClaimTypes.Name, "bookstoreuser"));
            identity.AddClaim(new Claim("sub", UserId));
            identity.AddClaim(new Claim("given_name", "Bookstore"));
            identity.AddClaim(new Claim("family_name", "User"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Administrators"));

            await Context.SignInAsync(new ClaimsPrincipal(identity));

            await SaveCustomerDetailsAsync(Context, identity);

            var redirectUrl = properties.RedirectUri ?? $"{Context.Request.Scheme}://{Context.Request.Host}{Context.Request.Path}{Context.Request.QueryString}";

            Context.Response.Redirect(redirectUrl);
        }

        private async Task SaveCustomerDetailsAsync(HttpContext context, ClaimsIdentity identity)
        {
            var customerService = context.RequestServices.GetService<ICustomerService>();

            var dto = new CreateOrUpdateCustomerDto(
                identity.FindFirst("Sub").Value,
                identity.Name,
                identity.FindFirst("given_name").Value,
                identity.FindFirst("family_name").Value);

            await customerService.CreateOrUpdateCustomerAsync(dto);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}