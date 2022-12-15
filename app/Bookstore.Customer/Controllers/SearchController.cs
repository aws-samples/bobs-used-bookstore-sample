using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Services;
using Bookstore.Customer;
using Services;
using Bookstore.Customer.Mappers;

namespace BobCustomerSite.Controllers
{

    public class SearchController : Controller
    {
        private readonly IInventoryService inventoryService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IShoppingCartClientManager shoppingCartClientManager;

        public SearchController(IInventoryService inventoryService,
                                IShoppingCartService shoppingCartService,
                                IShoppingCartClientManager shoppingCartClientManager)
        {
            this.inventoryService = inventoryService;
            this.shoppingCartService = shoppingCartService;
            this.shoppingCartClientManager = shoppingCartClientManager;
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
            var shoppingCartClientId = shoppingCartClientManager.GetShoppingCartId();

            await shoppingCartService.AddToShoppingCartAsync(shoppingCartClientId, bookId, 1);

            return RedirectToAction("Index", "Search");
        }

        public async Task<IActionResult> AddItemToWishlist(int bookId)
        {
            var shoppingCartClientId = shoppingCartClientManager.GetShoppingCartId();

            await shoppingCartService.AddToWishlistAsync(shoppingCartClientId, bookId);

            return RedirectToAction("Index", "Search");
        }
    }
}
