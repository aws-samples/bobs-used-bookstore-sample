using Bookstore.Customer.Mappers;
using Bookstore.Customer.ViewModel;
using Bookstore.Customer.ViewModel.Checkout;
using Bookstore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Customer.Controllers
{
    [Authorize]
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

            return RedirectToAction("OrderPlaced", new { orderId });
        }

        public IActionResult OrderPlaced(int orderId)
        {
            var order = orderService.GetOrder(orderId);

            var orderItemViewModels = order.OrderItems.Select(c => new OrderDetailViewModel
            {
                Bookname = c.Book.Name,
                Url = c.Book.FrontImageUrl,
                Price = c.Book.Price,
                Quantity = c.Quantity
            });

            ViewData["order"] = orderItemViewModels.ToList();

            return View();
        }
    }
}
