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
        [Key]
        public long Order_Id { get; set; }
        public Address Address { get; set; }
        public Customer Customer { get; set; }
        public double Subtotal { get; set; }
        public double Tax { get; set; }
        public OrderStatus OrderStatus { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string DeliveryDate { get; set; }
    }
}
