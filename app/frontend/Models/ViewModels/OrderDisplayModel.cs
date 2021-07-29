using BobsBookstore.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreFrontend.Models.ViewModels
{
    public class OrderDisplayModel
    {
        public IEnumerable<OrderDetail> OrderBookDetails { get; set; }
        public Order OrderDetail { get; set; }
    }
}
