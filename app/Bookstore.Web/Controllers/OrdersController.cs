using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Web.Helpers;
using Bookstore.Domain.Orders;
using Bookstore.Web.ViewModel.Orders;

namespace Bookstore.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await orderService.GetOrdersAsync(User.GetSub());

            return View(new OrderIndexViewModel(orders));
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await orderService.GetOrderAsync(id);

            return View(new OrderDetailsViewModel(order));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = new CancelOrderDto(User.GetSub(), id);

            await orderService.CancelOrderAsync(dto);

            return RedirectToAction("Index");
        }
    }
}