using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.Order
{
    public class Order
    {
        [Key]
        public long Order_Id { get; set; }

        public double Subtotal { get; set; }

        public double Tax { get; set; }

        public string DeliveryDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public BobBookstore.Models.Customer.Customer Customer { get; set; }

        public BobBookstore.Models.Customer.Address Address { get; set; }

        [Timestamp]
        public byte[] Rowversion { get; set; }
    }
}
