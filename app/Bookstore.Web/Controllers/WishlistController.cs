using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bookstore.Web.Helpers;
using Bookstore.Domain.Customers;
using Bookstore.Domain.Carts;
using Bookstore.Web.ViewModel.Wishlist;

namespace Bookstore.Web.Controllers
{
    [AllowAnonymous]
    public class WishlistController : Controller
    {
        private readonly IShoppingCartService shoppingCartService;

        public WishlistController(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index()
        {
            var shoppingCart = await shoppingCartService.GetShoppingCartAsync(HttpContext.GetShoppingCartCorrelationId());

            return View(new WishlistIndexViewModel(shoppingCart));
        }

        [HttpPost]
        public async Task<IActionResult> MoveToShoppingCart(int shoppingCartItemId)
        {
            var dto = new MoveWishlistItemToShoppingCartDto(HttpContext.GetShoppingCartCorrelationId(), shoppingCartItemId);

            await shoppingCartService.MoveWishlistItemToShoppingCartAsync(dto);

            this.SetNotification("Item moved to shopping cart");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> MoveAllItemsToShoppingCart()
        {
            var dto = new MoveAllWishlistItemsToShoppingCartDto(HttpContext.GetShoppingCartCorrelationId());

            await shoppingCartService.MoveAllWishlistItemsToShoppingCartAsync(dto);

            this.SetNotification("All items moved to shopping cart");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int shoppingCartItemId)
        {
            var dto = new DeleteShoppingCartItemDto(HttpContext.GetShoppingCartCorrelationId(), shoppingCartItemId);

            await shoppingCartService.DeleteShoppingCartItemAsync(dto);

            this.SetNotification("Item removed from wishlist");

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
