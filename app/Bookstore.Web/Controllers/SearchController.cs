using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Services;
using Services;
using Microsoft.AspNetCore.Authorization;
using Bookstore.Web.Mappers;
using Bookstore.Web.Helpers;

namespace Bookstore.Web.Controllers
{
    [AllowAnonymous]
    public class SearchController : Controller
    {
        private readonly IInventoryService inventoryService;
        private readonly IShoppingCartService shoppingCartService;

        public SearchController(IInventoryService inventoryService, IShoppingCartService shoppingCartService)
        {
            this.inventoryService = inventoryService;
            this.shoppingCartService = shoppingCartService;
        }

        public IActionResult Index(string searchString, string sortBy = "Name", int pageIndex = 1, int pageSize = 10)
        {
            var books = inventoryService.GetBooks(searchString, sortBy, pageIndex, pageSize);

            return View(books.ToSearchIndexViewModel());
        }

        public IActionResult Details(int id)
        {
            var book = inventoryService.GetBook(id);

            return View(book.ToSearchDetailsViewModel());
        }

        public async Task<IActionResult> AddItemToShoppingCart(int bookId)
        {
            await shoppingCartService.AddToShoppingCartAsync(HttpContext.GetShoppingCartId(), bookId, 1);

            this.SetNotification("Item added to shopping cart.");

            return RedirectToAction("Index", "Search");
        }

        public async Task<IActionResult> AddItemToWishlist(int bookId)
        {
            await shoppingCartService.AddToWishlistAsync(HttpContext.GetShoppingCartId(), bookId);

            this.SetNotification("Item added to wishlist.");

            return RedirectToAction("Index", "Search");
        }
    }
}
