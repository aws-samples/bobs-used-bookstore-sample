using System.ComponentModel.DataAnnotations;
using BobsBookstore.Models.Customers;


namespace BobsBookstore.Models.Orders
{
    public class Order
    {
        [Key]
        public long Order_Id { get; set; }

        public double Subtotal { get; set; }

        public double Tax { get; set; }

        public string DeliveryDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public BobsBookstore.Models.Customers.Customer Customer { get; set; }

        public Address Address { get; set; }

        [Timestamp]
        public byte[] Rowversion { get; set; }
    }
}
