using Bookstore.Domain.Books;
using Bookstore.Domain.Offers;
using Bookstore.Domain.Orders;
using Bookstore.Web.Areas.Admin.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookstore.Web.Areas.Admin.Controllers
{
    public class DashboardController : AdminAreaControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IOfferService offerService;
        private readonly IBookService bookService;

        public DashboardController(IOrderService orderService, IOfferService offerService, IBookService bookService)
        {
            this.orderService = orderService;
            this.offerService = offerService;
            this.bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            var orderStats = await orderService.GetStatisticsAsync();
            var offerStats = await offerService.GetStatisticsAsync();
            var inventoryStats = await bookService.GetStatisticsAsync();

            var model = new DashboardIndexViewModel
            {
                PastDueOrders = orderStats.PastDueOrders,
                PendingOrders = orderStats.PendingOrders,
                OrdersThisMonth = orderStats.OrdersThisMonth,
                OrdersTotal = orderStats.OrdersTotal,

                PendingOffers = offerStats.PendingOffers,
                OffersThisMonth = offerStats.OffersThisMonth,
                OffersTotal = offerStats.OffersTotal,

                LowStock = inventoryStats.LowStock,
                OutOfStock = inventoryStats.OutOfStock,
                StockTotal = inventoryStats.StockTotal
            };

            return View(model);
        }
    }
}
