using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models
{
    public class Cart
    {
        [Key]
        public long Cart_Id { get; set; }
        public Customer Customer { get; set; }
        public int Ip { get; set; }
    }
}
