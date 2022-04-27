using System.Collections.Generic;
using BobsBookstore.Models.Books;
using BobsBookstore.Models.Carts;
using BobsBookstore.Models.Customers;

namespace BobsBookstore.Models
{
    public class CartAllmodels
    {
        public CartAllmodels()
        {
            var db1 = new Cart();
        }

        public IEnumerable<Cart> Cart { get; set; }
        public IEnumerable<Customer> Customer { get; set; }
        public IEnumerable<CartItem> CartItem { get; set; }
        public IEnumerable<Book> Book { get; set; }
        public IEnumerable<Price> Price { get; set; }
    }
}