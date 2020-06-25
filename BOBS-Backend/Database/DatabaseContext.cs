using BOBS_Backend.Models.Book;
using BOBS_Backend.Models.Customer;
using BOBS_Backend.Models.Order;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Database
{
    public class DatabaseContext : DbContext
    {

        public DbSet<Book> Book { get; set; }
        public DbSet<Condition> Condition { get; set; }
        public DbSet<Genere> Genere { get; set; }
        public DbSet<Price> Price { get; set; }
        public DbSet<Models.Book.Type> Type { get; set; }


        public DbSet<Address> Address { get; set; }
        public DbSet<Customer> Customer { get; set; }


        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderStatus> orderStatus { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasOne<Order>(c => c.Order)
                .WithOne(ca => ca.Customer)
                .HasForeignKey<Order>(ca => ca.Customer_Id);

            modelBuilder.Entity<Address>()
                .HasOne<Order>(a => a.Order)
                .WithOne(aa => aa.Address)
                .HasForeignKey<Order>(ca => ca.Address_Id);
        }
    }
}
