using System;
using Bookstore.Domain.Addresses;
using Bookstore.Domain.Books;
using Bookstore.Domain.Carts;
using Bookstore.Domain.Customers;
using Bookstore.Domain.Offers;
using Bookstore.Domain.Orders;
using Bookstore.Domain.ReferenceData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bookstore.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Address> Address { get; set; }

        public DbSet<Book> Book { get; set; }

        public DbSet<Customer> Customer { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ShoppingCart> ShoppingCart { get; set; }

        public DbSet<ShoppingCartItem> ShoppingCartItem { get; set; }

        public DbSet<OrderItem> OrderItem { get; set; }

        public DbSet<Offer> Offer { get; set; }

        public DbSet<ReferenceDataItem> ReferenceData { get; set; }

        // The Aurora PostgreSQL schema produced by the AWS Transform schema conversion.
        private const string TargetSchema = "bobsusedbookstore_dbo";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(TargetSchema);

            // Case-insensitive text, matching the CITEXT columns in the converted schema.
            modelBuilder.HasPostgresExtension("citext");

            modelBuilder.Entity<Customer>().HasIndex(x => x.Sub).IsUnique();

            modelBuilder.Entity<Book>().HasOne(x => x.Publisher).WithMany().HasForeignKey(x => x.PublisherId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Book>().HasOne(x => x.BookType).WithMany().HasForeignKey(x => x.BookTypeId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Book>().HasOne(x => x.Genre).WithMany().HasForeignKey(x => x.GenreId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Book>().HasOne(x => x.Condition).WithMany().HasForeignKey(x => x.ConditionId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Offer>().HasOne(x => x.Publisher).WithMany().HasForeignKey(x => x.PublisherId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Offer>().HasOne(x => x.BookType).WithMany().HasForeignKey(x => x.BookTypeId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Offer>().HasOne(x => x.Genre).WithMany().HasForeignKey(x => x.GenreId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Offer>().HasOne(x => x.Condition).WithMany().HasForeignKey(x => x.ConditionId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<Order>().HasOne(x => x.Customer).WithMany().OnDelete(DeleteBehavior.Restrict);

            PopulateDatabase(modelBuilder);

            ApplyPostgreSqlConventions(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Aligns the model with the Aurora PostgreSQL schema produced by the AWS Transform
        /// schema conversion. The conversion emitted unquoted, lower-case identifiers, which
        /// PostgreSQL stores in lower case, whereas EF Core quotes identifiers and would
        /// otherwise look for the original PascalCase names. This is applied as a sweep over
        /// the model so that both convention-derived names and explicit [Column] attributes
        /// are normalised consistently.
        /// </summary>
        private static void ApplyPostgreSqlConventions(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName != null)
                {
                    entityType.SetTableName(tableName.ToLowerInvariant());
                }

                foreach (var property in entityType.GetProperties())
                {
                    var columnName = property.GetColumnName();
                    if (columnName != null)
                    {
                        property.SetColumnName(columnName.ToLowerInvariant());
                    }

                    // The converted schema uses CITEXT for every character column.
                    if (property.ClrType == typeof(string))
                    {
                        property.SetColumnType("citext");
                    }

                    // The converted schema uses NUMERIC(18,2) for monetary columns.
                    if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                    {
                        property.SetPrecision(18);
                        property.SetScale(2);
                    }

                    // The converted schema uses TIMESTAMP(6) WITHOUT TIME ZONE. Npgsql would
                    // otherwise map DateTime to "timestamp with time zone". This is set on the
                    // model rather than relying solely on the legacy-timestamp AppContext switch,
                    // so the mapping holds for any host that builds this model.
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("timestamp(6) without time zone");
                    }
                }

                // The converted schema declares identity columns as GENERATED ALWAYS.
                var primaryKey = entityType.FindPrimaryKey();
                if (primaryKey == null) continue;

                foreach (var property in primaryKey.Properties)
                {
                    if (property.ClrType == typeof(int) && property.ValueGenerated == ValueGenerated.OnAdd)
                    {
                        property.SetValueGenerationStrategy(NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);
                    }
                }
            }
        }
    }
}