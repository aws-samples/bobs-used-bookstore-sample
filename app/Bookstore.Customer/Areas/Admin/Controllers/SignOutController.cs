using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Bookstore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SignOutController : Controller
    {
        private IWebHostEnvironment webHostEnvironment;

        public SignOutController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult LogOut()
        {
            return webHostEnvironment.IsDevelopment() ? LocalSignOut() : CognitoSignOut();
        }

        private IActionResult LocalSignOut()
        {
            HttpContext.SignOutAsync("localauth");

            return RedirectToAction("Index", "Home");
        }

        private IActionResult CognitoSignOut()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
