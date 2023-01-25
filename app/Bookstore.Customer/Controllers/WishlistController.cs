using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Services;
using Microsoft.AspNetCore.Authorization;
using Bookstore.Web;
using Bookstore.Web.Mappers;

namespace Bookstore.Web.Controllers
{
    [AllowAnonymous]
    public class WishlistController : Controller
    {
        private readonly ICustomerService customerService;
        private readonly IShoppingCartService shoppingCartService;

        public WishlistController(ICustomerService customerService, IShoppingCartService shoppingCartService)
        {
            this.customerService = customerService;
            this.shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var wishlistItems = shoppingCartService.GetWishlistItems(HttpContext.GetShoppingCartId());

            return View(wishlistItems.ToWishlistIndexViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> MoveToShoppingCart(int shoppingCartItemId)
        {
            await shoppingCartService.MoveWishlistItemToShoppingCartAsync(HttpContext.GetShoppingCartId(), shoppingCartItemId);

            this.SetNotification("Item moved to shopping cart.");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> MoveAllItemsToShoppingCart()
        {
            await shoppingCartService.MoveAllWishlistItemsToShoppingCartAsync(HttpContext.GetShoppingCartId());

            this.SetNotification("All items moved to shopping cart.");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int shoppingCartItemId)
        {
            await shoppingCartService.DeleteShoppingCartItemAsync(HttpContext.GetShoppingCartId(), shoppingCartItemId);

            this.SetNotification("Item removed from wishlist.");

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
