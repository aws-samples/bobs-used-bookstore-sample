using Bookstore.Domain.Customers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Domain.Addresses
{
    [Table("address", Schema = "bobsusedbookstore_dbo")]
    public class Address : Entity
    {
        // An empty constructor is required by EF Core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Address() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Address(Customer customer, string addressLine1, string? addressLine2, string city, string state, string country, string zipCode)
        {
            Customer = customer;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            CustomerId = customer.Id;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }

        [Column("addressline1")]
        public string AddressLine1 { get; set; }

        [Column("addressline2")]
        public string? AddressLine2 { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("state")]
        public string State { get; set; }

        [Column("country")]
        public string Country { get; set; }

        [Column("zipcode")]
        public string ZipCode { get; set; }

        [Column("customerid")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; } = true;
    }
}