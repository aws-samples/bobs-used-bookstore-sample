using Bookstore.Domain.Addresses;
using Bookstore.Domain.Carts;
using Bookstore.Domain.Orders;
using Bookstore.Web.Helpers;
using Bookstore.Web.ViewModel.Checkout;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookstore.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IAddressService addressService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IOrderService orderService;

        public CheckoutController(IShoppingCartService shoppingCartService,
                                  IOrderService orderService,
                                  IAddressService addressService)
        {
            this.shoppingCartService = shoppingCartService;
            this.orderService = orderService;
            this.addressService = addressService;
        }

        public async Task<IActionResult> Index()
        {
            var shoppingCart = await shoppingCartService.GetShoppingCartAsync(HttpContext.GetShoppingCartCorrelationId());
            var addresses = await addressService.GetAddressesAsync(User.GetSub());

            return View(new CheckoutIndexViewModel(shoppingCart, addresses));
        }

        [HttpPost]
        public async Task<IActionResult> Index(CheckoutIndexViewModel model)
        {
            var dto = new CreateOrderDto(User.GetSub(), HttpContext.GetShoppingCartCorrelationId(), model.SelectedAddressId);

            var orderId = await orderService.CreateOrderAsync(dto);

            return RedirectToAction("Finished", new { orderId });
        }

        public async Task<IActionResult> Finished(int orderId)
        {
            var order = await orderService.GetOrderAsync(orderId);

            return View(new CheckoutFinishedViewModel(order));
        }
    }
}
