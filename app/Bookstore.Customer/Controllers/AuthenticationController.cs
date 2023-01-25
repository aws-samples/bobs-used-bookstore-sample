using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Bookstore.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private IWebHostEnvironment webHostEnvironment;

        public AuthenticationController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        public IActionResult Login(string redirectUri = null)
        {
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUri ?? Request.GetTypedHeaders().Referer.ToString() });
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
