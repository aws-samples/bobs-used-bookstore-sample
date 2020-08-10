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
using BOBS_Backend.Models.Book;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BOBS_Backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ICustomAdminPage _customeAdmin;
        private string adminUsername;

        public HomeController(ICustomAdminPage customAdmin)
        {
            _customeAdmin = customAdmin;
           
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult WelcomePage(string sortByValue)
        {

            adminUsername = User.Claims.FirstOrDefault(c => c.Type.Equals("cognito:username"))?.Value;
            LatestUpdates bookUpdates = new LatestUpdates();
            // the sortByValue input for different sorting parameters. 
            List<string> orderByMethods = new List<string>{"price", "price_desc", "date", "date_desc"};
            // assigns ViewBag a default value just to initialize it 
            ViewBag.SortPrice = "price";
            ViewBag.SortDate = "date";
            ViewBag.SortStatus = "status";
            ViewBag.PriceArrow = "▲";
            ViewBag.DateArrow = "▲";
            ViewBag.StatusArrow = "▲";
            // get the date range from the user
            int dateMinRange = 0;
            int dateMaxRange = 5;

            //Get books updated by current user
            bookUpdates.UserBooks = _customeAdmin.GetUserUpdatedBooks(adminUsername).Result;
            // get recent books updated globally
            bookUpdates.NotUserBooks = _customeAdmin.OtherUpdatedBooks(adminUsername).Result;
            // get important orders
            bookUpdates.ImpOrders = _customeAdmin.GetImportantOrders(dateMaxRange, dateMinRange).Result;
            if (bookUpdates.UserBooks == null || bookUpdates.NotUserBooks == null || bookUpdates.ImpOrders == null)
                throw new ArgumentNullException("The LatestUpdates View model cannot have a null value", "bookupdates");

            if (orderByMethods.Contains(sortByValue))
            {
                // assigns ViewBag.Sort with the opposite value of sortByValue
                if (sortByValue == "price" || sortByValue == "price_desc")
                    ViewBag.SortPrice = sortByValue == "price" ? "price_desc" : "price";
                else if (sortByValue == "date" || sortByValue == "date_desc")
                    ViewBag.SortDate = sortByValue == "date" ? "date_desc" : "date";
                else if (sortByValue == "status" || sortByValue == "status_desc")
                    ViewBag.SortDate = sortByValue == "status" ? "status_desc" : "status";
                //to change the arrow on the html anchors based on asc or desc
                if (ViewBag.SortPrice == "price")
                    ViewBag.PriceArrow = "▲";
                else if (ViewBag.SortPrice == "price_desc")
                    ViewBag.PriceArrow = "▼";
                if (ViewBag.SortDate == "date")
                    ViewBag.DateArrow = "▲";
                else if (ViewBag.SortDate == "date_desc")
                    ViewBag.DateArrow = "▼";
                if (ViewBag.SortStatus == "status")
                    ViewBag.DateArrow = "▲";
                else if (ViewBag.SortStatus == "status_desc")
                    ViewBag.DateArrow = "▼";

                bookUpdates.ImpOrders = _customeAdmin.SortTable(bookUpdates.ImpOrders, sortByValue);
                return View(bookUpdates);
            }
            else
            {
                
                return View(bookUpdates);
            }
            
            
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
