using BobBookstore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using BobBookstore.Models;
using BobBookstore.Models.Customer;
using BobBookstore.Models.Order;
using BobBookstore.Models.Book;

namespace BobBookstore.Data
{
    public class UsedBooksContext : DbContext
    {
        public UsedBooksContext()
        {
        }

        public UsedBooksContext(DbContextOptions<UsedBooksContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Condition> Condition { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Order> Order { get; set; }
        //public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<Price> Price { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<Models.Book.Type> Type { get; set; }
        public DbSet<BobBookstore.Models.Carts.Cart> Cart { get; set; }
        public DbSet<BobBookstore.Models.Carts.CartItem> CartItem { get; set; }
        public DbSet<Models.Order.OrderDetail> OrderDetail { get; set; }
    }
}
