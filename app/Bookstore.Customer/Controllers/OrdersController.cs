using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Customer;
using Microsoft.AspNetCore.Authorization;
using Bookstore.Services;
using Bookstore.Customer.Mappers;

namespace CustomerSite.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public IActionResult Index()
        {
            var orders = orderService.GetOrders(User.GetSub());

            return View(orders.ToOrderIndexViewModel());
        }

        public IActionResult Details(int id)
        {
            var order = orderService.GetOrder(id);

            return View(order.ToOrderDetailsViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await orderService.CancelOrderAsync(id, User.GetSub());

            return RedirectToAction("Index");
        }
    }
}