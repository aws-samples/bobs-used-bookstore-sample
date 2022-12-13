using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Bookstore.Customer.Controllers
{
    public class AuthenticationController : Controller
    {
        private IWebHostEnvironment webHostEnvironment;

        public AuthenticationController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = Request.GetTypedHeaders().Referer.ToString() });
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
