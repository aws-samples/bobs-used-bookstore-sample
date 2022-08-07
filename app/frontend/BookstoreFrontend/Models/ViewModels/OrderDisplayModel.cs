using System.Collections.Generic;
using BobsBookstore.Models.Orders;

namespace BookstoreFrontend.Models.ViewModels
{
    public class OrderDisplayModel
    {
        public IEnumerable<OrderDetail> OrderBookDetails { get; set; }
        public Order OrderDetail { get; set; }
    }
}