using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bookstore.Customer.Controllers
{
    public class AuthenticationController : Controller
    {
        private IWebHostEnvironment webHostEnvironment;
        private const string UserId = "FB6135C7-1464-4A72-B74E-4B63D343DD09";

        public AuthenticationController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Login()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = Request.GetTypedHeaders().Referer.ToString() });
        }

        public IActionResult LogOut()
        {
            return webHostEnvironment.IsDevelopment() ? LocalSignOut() : CognitoSignOut();
        }

        private IActionResult LocalSignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        private IActionResult CognitoSignOut()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
