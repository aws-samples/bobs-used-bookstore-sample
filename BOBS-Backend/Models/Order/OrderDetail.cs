using BOBS_Backend.Models.Book;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Models.Order
{
    public class OrderDetail
    {
        [Key]
        public long OrderDetail_Id { get; set; }

        public Order Order { get; set; }

        public Book.Book Book { get; set; }

        public Price Price { get; set; }

        public double price { get; set; }

        public int quantity { get; set; }
    }
}
