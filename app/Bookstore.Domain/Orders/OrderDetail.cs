using Bookstore.Domain.Books;

namespace Bookstore.Domain.Orders
{
    public class OrderItem : Entity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int Quantity { get; set; }

        public bool IsRemoved { get; set; }
    }
}