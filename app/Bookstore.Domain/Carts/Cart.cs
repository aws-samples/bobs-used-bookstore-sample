using System.ComponentModel.DataAnnotations;
using Bookstore.Domain.Customers;

namespace Bookstore.Domain.Carts
{
    public class Cart
    {
        [Key] public string Cart_Id { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public string? IP { get; set; }
    }
}