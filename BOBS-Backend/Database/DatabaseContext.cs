using BOBS_Backend.Models.Book;
using BOBS_Backend.Models.Customer;
using BOBS_Backend.Models.Order;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Database
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
