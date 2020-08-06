using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.Carts
{
    public class Cart
    {
        [Key]
        public int Cart_Id { get; set; }
        public Customer.Customer Customer { get; set; }
        public string IP { get; set; }

    }
}
