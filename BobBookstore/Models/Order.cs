using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models
{
    public class Order
    {
        [Required]
        public int ID { get; set; }
        public int AddressID { get; set; }
        public int CustomerID { get; set; }
        public int Subtotal { get; set; }
        public int Tax { get; set; }
        public int OrderStatusID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string DeliveryDate { get; set; }
    }
}
