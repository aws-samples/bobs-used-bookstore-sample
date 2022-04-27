using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BobsBookstore.Models.Customers;

namespace BobsBookstore.Models.Orders
{
    public class Order
    {
        [Key] public long Order_Id { get; set; }

        [Column(TypeName = "decimal(18,2)")] public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")] public decimal Tax { get; set; }

        public string DeliveryDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public Customer Customer { get; set; }

        public Address Address { get; set; }

        [Timestamp] public byte[] Rowversion { get; set; }
    }
}