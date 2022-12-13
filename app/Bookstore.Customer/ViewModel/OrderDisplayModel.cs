using System.Collections.Generic;
using Bookstore.Domain.Orders;

namespace Bookstore.Customer.ViewModel
{
    public class OrderDisplayModel
    {
        public IEnumerable<OrderDetail> OrderBookDetails { get; set; }
        public Order OrderDetail { get; set; }
    }
}