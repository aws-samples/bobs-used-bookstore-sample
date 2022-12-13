using System.Collections.Generic;
using Bookstore.Domain.Orders;

namespace CustomerSite.Models.ViewModels
{
    public class OrderDisplayModel
    {
        public IEnumerable<OrderDetail> OrderBookDetails { get; set; }
        public Order OrderDetail { get; set; }
    }
}