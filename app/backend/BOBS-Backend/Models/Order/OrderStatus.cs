using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Models.Order
{
    public class OrderStatus
    {
        /*
         * OrderStatus Model 
         */

        [Key]
        public long OrderStatus_Id { get; set; }

        public string Status { get; set; }

        public int position { get; set; }

    }
}
