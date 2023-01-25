using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Services;
using Microsoft.AspNetCore.Authorization;
using Bookstore.Web.Mappers;
using Bookstore.Web.Helpers;

namespace Bookstore.Web.Controllers
{
    [AllowAnonymous]
    public class ShoppingCartController : Controller
    {
        private readonly ICustomerService customerService;
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartController(ICustomerService customerService, IShoppingCartService shoppingCartService)
        {
            this.customerService = customerService;
            this.shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var shoppingCartItems = shoppingCartService.GetShoppingCartItems(HttpContext.GetShoppingCartId());

            return View(shoppingCartItems.ToShoppingCartIndexViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int shoppingCartItemId)
        {
            await shoppingCartService.DeleteShoppingCartItemAsync(HttpContext.GetShoppingCartId(), shoppingCartItemId);

            this.SetNotification("Item removed from shopping cart.");

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
