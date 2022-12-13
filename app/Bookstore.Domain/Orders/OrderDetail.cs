using Bookstore.Domain.Books;

namespace Bookstore.Domain.Orders
{
    public class OrderDetail : Entity
    {
        public Order Order { get; set; }

        public Book Book { get; set; }

        public decimal OrderDetailPrice { get; set; }

        public int Quantity { get; set; }

        public bool IsRemoved { get; set; }
    }
}