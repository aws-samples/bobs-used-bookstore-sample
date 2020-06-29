using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models
{
    public class OrderDetail
    {
        [Required]
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int BookID { get; set; }
        public int PriceID { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
