using Bookstore.Domain.Addresses;
using Bookstore.Domain.Books;
using Bookstore.Domain.Customers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Bookstore.Domain.Orders
{
    [Table("order", Schema = "public")]
    public class Order : Entity
    {
        public Order(int customerId, int addressId)
        {
            CustomerId = customerId;
            AddressId = addressId;
        }

        private readonly List<OrderItem> orderItems = new List<OrderItem>();

        [Column("customer_id")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Column("address_id")]
        public int AddressId { get; set; }
        public Address Address { get; set; }

        [NotMapped]
        public IEnumerable<OrderItem> OrderItems => orderItems;

        [Column("delivery_date")]
        public DateTime DeliveryDate { get; set; } = DateTime.Now.ToUniversalTime().AddDays(7);

        [Column("order_status")]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        [NotMapped]
        public decimal Tax => SubTotal * 0.1m;

        [NotMapped]
        public decimal SubTotal => OrderItems.Sum(x => x.Book.Price);

        [NotMapped]
        public decimal Total => SubTotal + Tax;

        public void AddOrderItem(Book book, int quantity)
        {
            orderItems.Add(new OrderItem(this, book, quantity));
        }
    }
}
