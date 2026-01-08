using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bookstore.Domain.Addresses;
using Bookstore.Domain.Books;
using Bookstore.Domain.Customers;

namespace Bookstore.Domain.Orders
{
    [Table("orders", Schema = "bobsusedbookstore_dbo")]
    public class Order : Entity
    {
        public Order(int customerId, int addressId)
        {
            CustomerId = customerId;
            AddressId = addressId;
        }

        private readonly List<OrderItem> orderItems = new List<OrderItem>();

        [Column("customerid")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Column("addressid")]
        public int AddressId { get; set; }
        public Address Address { get; set; }

        [NotMapped]
        public IEnumerable<OrderItem> OrderItems => orderItems;

        [Column("deliverydate")]
        public DateTime DeliveryDate { get; set; } = DateTime.UtcNow.AddDays(7);

        [Column("orderstatus")]
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