using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bookstore.Domain.Books;

namespace Bookstore.Domain.Orders
{
    [Table("orderitem", Schema = "bobsusedbookstore_dbo")]
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

        [Column("orderid")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Column("bookid")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }
    }
}
