using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobBookstore.Data;
using BobBookstore.Models;
using BobBookstore.Models.Book;
using Microsoft.EntityFrameworkCore;
namespace BobBookstore.Models
{
    public class CartAllmodels
    {
        public IEnumerable<Models.Carts.Cart> Cart { get; set; }
        public IEnumerable<Customer.Customer> Customer { get; set; }
        public IEnumerable<Carts.CartItem> CartItem { get; set; }
        public IEnumerable<Book.Book> Book { get; set; }
        public IEnumerable<Book.Price> Price { get; set; }

        public CartAllmodels()
        {
            //var db = new UsedBooksContext();
            Carts.Cart db1 = new Carts.Cart();
            
            

        }
    }
}
