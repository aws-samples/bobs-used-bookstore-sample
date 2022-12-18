using Bookstore.Domain.Customers;

namespace Bookstore.Domain.Orders
{
    public class Order : Entity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int? AddressId { get; set; }
        public Address? Address { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public DateTime DeliveryDate { get; set; } = DateTime.Now.AddDays(7);

        public OrderStatus OrderStatus { get; set; }

        public decimal Tax => SubTotal * 0.1m;

        public decimal SubTotal => OrderItems.Sum(x => x.Book.Price);

        public decimal Total => SubTotal + Tax;
    }
}