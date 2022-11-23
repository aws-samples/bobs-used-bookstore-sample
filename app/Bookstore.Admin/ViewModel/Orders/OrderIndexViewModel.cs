using Bookstore.Domain.Orders;
using System;
using System.Collections.Generic;

namespace Bookstore.Admin.ViewModel.Orders
{
    public class OrderIndexViewModel : PaginatedViewModel
    {
        public List<OrderIndexListItemViewModel> Items { get; set; } = new List<OrderIndexListItemViewModel>();

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