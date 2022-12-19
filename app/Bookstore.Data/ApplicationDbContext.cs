using System;
using System.IO;
using Bookstore.Domain.Books;
using Bookstore.Domain.Carts;
using Bookstore.Domain.Customers;
using Bookstore.Domain.Offers;
using Bookstore.Domain.Orders;
using Bookstore.Domain.ReferenceData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using BookType = Bookstore.Domain.Books.BookType;

namespace Bookstore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Condition> Condition { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Price> Price { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<BookType> Type { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItem { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Offer> Offer { get; set; }
        public DbSet<ReferenceDataItem> ReferenceData { get; set; }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var environmentName =
               Environment.GetEnvironmentVariable(
                   "ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Directory.GetCurrentDirectory() + "/../frontend/appsettings.json")
                .AddJsonFile(Directory.GetCurrentDirectory() + $"/../frontend/appsettings.{environmentName}.json", true)

                .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("BobBookstoreContextConnection");
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("DataMigrations"));

            return new ApplicationDbContext(builder.Options);
        }
    }
}
