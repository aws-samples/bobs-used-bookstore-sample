using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.Order
{
    public class OrderDetails
    {
        [Key]
        public long OrderDetail_Id { get; set; }

        public Order Order { get; set; }

        public Models.Book.Book Book { get; set; }

        public Models.Book.Price Price { get; set; }

        public double price { get; set; }

        public int Quantity
        {
            get; set;
        }
    }
}
