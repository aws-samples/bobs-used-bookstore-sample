using System;
using System.Collections.Generic;

namespace Bookstore.Customer.ViewModel.Orders
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }

        public string OrderStatus { get; set; }

        public DateTime DeliveryDate { get; set; }

        public decimal Total { get; set; }

        public List<OrderDetailsItemViewModel> OrderItems { get; set; } = new List<OrderDetailsItemViewModel>();
    }

    public class OrderDetailsItemViewModel
    {
        public int BookId { get; set; }

        public string ImageUrl { get; set; }

        public string BookName { get; set; }

        public decimal Price { get; set; }
    }
}