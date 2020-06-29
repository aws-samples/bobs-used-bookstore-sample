using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models
{
    public class Price
    {
        [Required]
        public int ID { get; set; }
        public int BookID { get; set; }
        public int ConditionID { get; set; }
        public int price { get; set; }
        public int Quantity { get; set; }
    }
}
