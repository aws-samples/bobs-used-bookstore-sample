using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BOBS_Backend.Models;
using BOBS_Backend.Controllers.LoginActions;
namespace BOBS_Backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult WelcomePage([Bind] BOBS_Backend.Models.AdminUser ad)
        {
            VerifyLogin auth = new VerifyLogin();
            TempData["Username"] = ad.Username;
            bool res = auth.authenticate(ad.Username, ad.password);
            if (res)
            {

                return View();
            }
            else
            {

                return RedirectToAction("Index");
            }

        }
        
        public IActionResult RegisterPage([Bind] BOBS_Backend.Models.AdminUser ad)
        {
            ViewData["Register"] = null;
            RegisterAdmin regAdmin = new RegisterAdmin();
            bool res = regAdmin.Register(ad.Username, ad.password, ad.email);
            if (res)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["Register"] = "Error in Registering";
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
