using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bookstore.Web.ViewModel;
using Bookstore.Domain.Books;
using System.Threading.Tasks;
using Bookstore.Web.ViewModel.Home;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;

namespace Bookstore.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IBookService bookService;
        private readonly ILogger<HomeController> logger;

        public HomeController(IBookService bookService, ILogger<HomeController> logger)
        {
            this.bookService = bookService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            logger.LogDebug("Retrieving best selling books to display on home page");
            var books = await bookService.ListBestSellingBooksAsync(4);

            logger.LogInformation("Retrieved the {Number} top selling books", 4);
            return View(new HomeIndexViewModel(books));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Search()
        {
            return RedirectToAction("Index", "Search");
        }

        public IActionResult Cart()
        {
            return RedirectToAction("Index", "ShoppingCart");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
