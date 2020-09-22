using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.Order
{
    public class OrderStatus
    {
        [Key]
        public long OrderStatus_Id { get; set; }

        public string Status { get; set; }

        public int position { get; set; }
    }
}
