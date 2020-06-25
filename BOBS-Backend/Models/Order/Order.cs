using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BOBS_Backend.Models.Customer;

namespace BOBS_Backend.Models.Order
{
    public class Order
    {
        [Key]
        public long Order_Id { get; set; }

        public double Subtotal { get; set; }

        public double Tax { get; set; }

        public string DeliveryDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public long Customer_Id { get; set; }

        public Customer.Customer Customer { get; set; }

        public long Address_Id { get; set; }

        public Address Address { get; set; }
    }
}
