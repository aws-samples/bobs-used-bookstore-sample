namespace Bookstore.Web.Areas.Admin.Models.Dashboard
{
    public class DashboardIndexViewModel
    {
        public int PendingOrders { get; set; }

        public int PastDueOrders { get; set; }

        public int OrdersThisMonth { get; set; }

        public int OrdersTotal { get; set; }
    }
}
