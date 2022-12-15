using Bookstore.Domain.Customers;

namespace Bookstore.Domain.Carts
{
    public class ShoppingCart : Entity
    {
        public string CorrelationId { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}