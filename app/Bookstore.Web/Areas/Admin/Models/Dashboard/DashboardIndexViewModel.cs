namespace Bookstore.Web.Areas.Admin.Models.Dashboard
{
    public class DashboardIndexViewModel
    {
        public int PendingOrders { get; set; }

        public int PastDueOrders { get; set; }

        public int OrdersThisMonth { get; set; }

        public int OrdersTotal { get; set; }

        public int PendingOffers { get; set; }

        public int PastDueOffers { get; set; }

        public int OffersThisMonth { get; set; }

        public int OffersTotal { get; set; }

        public int OutOfStock { get; set; }

        public int LowStock { get; set; }

        public int StockTotal { get; set; }
    }
}
