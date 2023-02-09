using Bookstore.Domain.Orders;
using Bookstore.Web.Areas.Admin.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookstore.Web.Areas.Admin.Controllers
{
    public class DashboardController : AdminAreaControllerBase
    {
        private readonly IOrderService orderService;

        public DashboardController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orderStats = await orderService.GetStatisticsAsync();
            var model = new DashboardIndexViewModel
            {
                PastDueOrders = orderStats.PastDueOrders,
                PendingOrders = orderStats.PendingOrders,
                OrdersThisMonth = orderStats.OrdersThisMonth,
                OrdersTotal = orderStats.OrdersTotal
            };

            return View(model);
        }
    }
}
