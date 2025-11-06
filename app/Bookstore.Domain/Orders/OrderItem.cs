using Bookstore.Domain.Books;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.Orders
{
    [Table("order_item", Schema = "public")]
    public class OrderItem : Entity
    {
        // This private constructor is required by EF Core
        private OrderItem() { }

        public OrderItem(Order order, Book book, int quantity)
        {
            OrderId = order.Id;
            Order = order;
            BookId = book.Id;
            Book = book;
            Quantity = quantity;
        }

        [Column("order_id")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Column("book_id")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }
    }
}