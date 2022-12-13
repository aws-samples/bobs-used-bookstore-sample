using Bookstore.Domain.Customers;

namespace Bookstore.Domain.Orders
{
    public class Order : Entity
    {
        public Customer Customer { get; set; }

        public Address? Address { get; set; }

        public DateTime DeliveryDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public decimal Tax { get; set; }

        public decimal Subtotal { get; set; }
    }
}