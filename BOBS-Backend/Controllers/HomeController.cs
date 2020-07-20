using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BOBS_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using BOBS_Backend.Models.AdminUser;
using BOBS_Backend.Repository.Implementations.AdminImplementation;
using BOBS_Backend.Repository.WelcomePageInterface;
using BOBS_Backend.Views.Orders.Shared;
using BOBS_Backend.Repository.Implementations.WelcomePageImplementation;
using BOBS_Backend.Repository.Implementations.WelcomePageImplementation;
using BOBS_Backend.ViewModel.UpdateBooks;

namespace BOBS_Backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ICustomAdminPage _customeAdmin;


        public HomeController(ICustomAdminPage customAdmin)
        {
            _customeAdmin = customAdmin;
           
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult WelcomePage()
        {
            BookUpdates bookUpdates = new BookUpdates();
            bookUpdates.books = _customeAdmin.GetUpdatedBooks(User.Claims).Result;
            bookUpdates.globalBooks = _customeAdmin.GetGlobalUpdatedBooks().Result;
            
             return View(bookUpdates);
        }
        public IActionResult Logout()
        {
            return View();
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
