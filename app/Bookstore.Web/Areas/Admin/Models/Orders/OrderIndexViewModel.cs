using Bookstore.Domain.Orders;
using Bookstore.Services.Filters;
using System;
using System.Collections.Generic;

namespace Bookstore.Web.Areas.Admin.Models.Orders
{
    public class OrderIndexViewModel : PaginatedViewModel
    {
        public List<OrderIndexListItemViewModel> Items { get; set; } = new List<OrderIndexListItemViewModel>();

        public OrderFilters Filters { get; set; }
    }

    public class OrderIndexListItemViewModel
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime DeliveryDate { get; set; }

        public DateTime OrderDate { get; internal set; }

        public decimal Total { get; internal set; }
    }
}