using Bookstore.Services;
using Bookstore.Web;
using Bookstore.Web.Mappers;
using Bookstore.Web.ViewModel.Checkout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookstore.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICustomerService customerService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IOrderService orderService;

        public CheckoutController(ICustomerService customerService,
                                  IShoppingCartService shoppingCartService,
                                  IOrderService orderService)
        {
            this.customerService = customerService;
            this.shoppingCartService = shoppingCartService;
            this.orderService = orderService;
        }

        public IActionResult Index()
        {
            var shoppingCart = shoppingCartService.GetShoppingCart(HttpContext.GetShoppingCartId());
            var addresses = customerService.GetAddresses(User.GetSub());
            var viewModel = shoppingCart.ToCheckoutIndexViewModel(addresses);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CheckoutIndexViewModel model)
        {
            var orderId = await orderService.CreateOrderAsync(HttpContext.GetShoppingCartId(), User.GetSub(), model.SelectedAddressId);

            return RedirectToAction("Finished", new { orderId });
        }

        public IActionResult Finished(int orderId)
        {
            var order = orderService.GetOrder(orderId);

            return View(order.ToCheckoutFinishedViewModel());
        }
    }
}
