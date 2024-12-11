using Bookstore.Domain.Customers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Bookstore.Web.Helpers
{
    public class LocalAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string UserId = "FB6135C7-1464-4A72-B74E-4B63D343DD09";

        public LocalAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Cookies.ContainsKey("BobsUsedBooks")) return AuthenticateResult.Fail("Not authenticated");

            ClaimsPrincipal claimsPrincipal = null;
            await Task.Run(() => {var claimsPrincipal = CreateClaimsPrincipal();});

            return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (Request.Path.Value != null && Request.Path.Value.StartsWith("/Authentication/Login"))
            {
                var claimsPrincipal = CreateClaimsPrincipal();

                await SaveCustomerDetailsAsync(claimsPrincipal);

                var userCookie = new CookieOptions { Secure = false, Expires = DateTime.Now.AddDays(1) };

                Response.Cookies.Append("BobsUsedBooks", "authenticated", userCookie);

                Response.Redirect("/");
            }
        }

        private ClaimsPrincipal CreateClaimsPrincipal()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "bookstoreuser"),
                new Claim("sub", UserId),
                new Claim("given_name", "Bookstore"),
                new Claim("family_name", "User"),
                new Claim(ClaimTypes.Role, "Administrators")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            
            return new ClaimsPrincipal(identity);
        }

        private async Task SaveCustomerDetailsAsync(ClaimsPrincipal principal)
        {
            var customerService = Request.HttpContext.RequestServices.GetService<ICustomerService>();

            var dto = new CreateOrUpdateCustomerDto(
                principal.FindFirst("Sub").Value,
                principal.Identity.Name,
                principal.FindFirst("given_name").Value,
                principal.FindFirst("family_name").Value);

            await customerService.CreateOrUpdateCustomerAsync(dto);
        }
    }
}