using Microsoft.EntityFrameworkCore;
using BobsBookstore.Models.Books;
using BobsBookstore.Models.Carts;
using BobsBookstore.Models.Customers;
using BobsBookstore.Models.Orders;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace BobsBookstore.DataAccess.Data
{
    public class ApplicationDbContext: DbContext
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
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<Price> Price { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<Type> Type { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../app/frontend/appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("BobBookstoreContextConnection");
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("BobsBookstore.Migrations"));
            
            return new ApplicationDbContext(builder.Options);
        }
    }
}
