using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Bookstore.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login(string redirectUri = null)
        {
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUri ?? Request.GetTypedHeaders().Referer.ToString() });
        }

        public async Task<IActionResult> LogOutAsync()
        {
            if (configuration["Services:Authentication"] == "aws") return CognitoLogOut();
            
            return await LocalLogOutAsync();
        }

        private async Task<IActionResult> LocalLogOutAsync()
        {
            if (!Request.Cookies.ContainsKey("BobsUsedBooks")) return RedirectToAction("Index", "Home");

            var userCookie = new CookieOptions { Secure = false, Expires = DateTime.Now.AddDays(-1) };

            Response.Cookies.Append("BobsUsedBooks", string.Empty, userCookie);

            return RedirectToAction("Index", "Home");
        }

        private IActionResult CognitoLogOut()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
