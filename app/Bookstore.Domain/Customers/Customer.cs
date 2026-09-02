using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.Customers
{
    [Table("customer", Schema = "bobsusedbookstore_dbo")]
    public class Customer : Entity
    {
        // Empty constructor required by EF Core
        public Customer() { }
        
        public Customer(string sub)
        {
            Sub = sub;
        }
        
        [Column("sub")]
        public string Sub { get; set; }

        [Column("username")]
        public string? Username { get; set; }

        [Column("firstname")]
        public string? FirstName { get; set; }

        [Column("lastname")]
        public string? LastName { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [Column("email")]
        public string? Email { get; set; }

        [Column("dateofbirth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }
    }
}
