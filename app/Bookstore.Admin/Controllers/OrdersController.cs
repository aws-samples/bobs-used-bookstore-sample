using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Services;
using Bookstore.Admin.Mappers.Orders;
using Bookstore.Admin.ViewModel.Orders;

namespace AdminSite.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public IActionResult Index(OrderIndexFilters filters, int pageIndex = 1, int pageSize = 10)
        {
            var orders = orderService.GetOrders(filters.OrderStatusFilter, pageIndex, pageSize);

            var model = orders.ToOrderIndexItemViewModel();

            model.Filters = filters;

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var order = orderService.GetOrder(id);
            var orderDetails = orderService.GetOrderDetails(id);
            var model = order.ToOrderDetailsViewModel();

            model.AddOrderDetails(orderDetails);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Details(OrderDetailsViewModel model)
        {
            var order = orderService.GetOrder(model.OrderId);

            order.OrderStatus = model.SelectedOrderStatus;

            await orderService.SaveOrderAsync(order, User.Identity.Name);

            TempData["Message"] = "Order status has been updated";

            return RedirectToAction("Details", new { model.OrderId });
        }
    }
}
