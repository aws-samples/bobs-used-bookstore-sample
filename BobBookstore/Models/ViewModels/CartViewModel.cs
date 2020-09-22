using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.ViewModels
{
    public class CartViewModel
    {
        public long BookId { get; set; }
        public string BookName { get; set; }
        public long ISBN { get; set; }
        public string GenreName { get; set; }
        public string TypeName { get; set; }
        public double Prices { get; set; }
        public string Url { get; set; }

        public string CartItem_Id { get; set; }

        public int quantity { get; set; }
        public long PriceId { get; set; }
    }
}
