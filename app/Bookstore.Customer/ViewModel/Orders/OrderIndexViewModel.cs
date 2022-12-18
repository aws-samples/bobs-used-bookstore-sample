using System;
using System.Collections.Generic;

namespace Bookstore.Customer.ViewModel.Orders
{
    public class OrderIndexViewModel
    {
        public List<OrderIndexItemViewModel> OrderItems { get; set; }
    }

    public class OrderIndexItemViewModel
    {
        public int Id { get; set; }

        public decimal SubTotal { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string OrderStatus { get; set; }
    }
}