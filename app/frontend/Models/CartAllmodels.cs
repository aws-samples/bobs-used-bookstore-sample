using BobsBookstore.Models.Books;
using BobsBookstore.Models.Customers;
using System.Collections.Generic;

namespace BobsBookstore.Models
{
    public class CartAllmodels
    {
        public IEnumerable<Models.Carts.Cart> Cart { get; set; }
        public IEnumerable<Customer> Customer { get; set; }
        public IEnumerable<Carts.CartItem> CartItem { get; set; }
        public IEnumerable<Book> Book { get; set; }
        public IEnumerable<Price> Price { get; set; }

        public CartAllmodels()
        {
            Carts.Cart db1 = new Carts.Cart();
        }
    }
}
