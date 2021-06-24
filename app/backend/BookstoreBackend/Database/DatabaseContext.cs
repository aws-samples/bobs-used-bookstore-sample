using Microsoft.EntityFrameworkCore;
using BookstoreBackend.Models.Book;
using BookstoreBackend.Models.Customer;
using BookstoreBackend.Models.Order;

namespace BookstoreBackend.Database
{
    public class DatabaseContext : DbContext
    {
        /*
         * This file contains all the tables (i.e. structure and name) used in the SQL Server
         */

        public DbSet<Book> Book { get; set; }
        public DbSet<Condition> Condition { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Price> Price { get; set; }

        public DbSet<Publisher> Publisher { get; set; }

        public DbSet<Models.Book.Type> Type { get; set; }


        public DbSet<Address> Address { get; set; }
        public DbSet<Customer> Customer { get; set; }


        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }


    }
}
