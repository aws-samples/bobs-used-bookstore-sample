using System.Threading.Tasks;
using AdminSite.ViewModel.ManageOrders;
using AdminSite.ViewModel.ProcessOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Bookstore.Data.Repository.Interface.OrdersInterface;
using Bookstore.Data.Repository.Interface.NotificationsInterface;
using Bookstore.Admin;
using Bookstore.Services;
using Bookstore.Admin.Mappers.Orders;
using Bookstore.Domain.Orders;
using Bookstore.Admin.ViewModel.Orders;

namespace AdminSite.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly INotifications _emailSender;
        private readonly IOrderRepository _order;
        private readonly IOrderDetailRepository _orderDetail;
        private readonly IOrderService orderService;

        public OrdersController()
        {
        }

        [ActivatorUtilitiesConstructor]
        public OrdersController(IOrderDetailRepository orderDetail,
            IOrderRepository order,
            INotifications emailSender,
            IOrderService orderService)
        {
            _orderDetail = orderDetail;
            _order = order;
            _emailSender = emailSender;

            this.orderService= orderService;
        }
        
        public IActionResult Index(int pageIndex = 1, int pageSize = 10)
        {
            var orders = orderService.GetOrders(User.Identity.Name, pageIndex, pageSize);

            return View(orders.ToOrderIndexViewModel());
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
