using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.Customers
{
    [Table("customer", Schema = "public")]
    public class Customer : Entity
    {
        [Column("sub")]
        public string Sub { get; set; }

        [Column("username")]
        public string? Username { get; set; }

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [Column("email")]
        public string? Email { get; set; }

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }
    }
}