using System;
using System.Collections.Generic;

namespace Bookstore.Web.ViewModel.Orders
{
    public class OrderIndexViewModel
    {
        public List<OrderIndexItemViewModel> OrderItems { get; set; } = new List<OrderIndexItemViewModel>();
    }

    public class OrderIndexItemViewModel
    {
        public int Id { get; set; }

        public decimal SubTotal { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string OrderStatus { get; set; }
    }
}