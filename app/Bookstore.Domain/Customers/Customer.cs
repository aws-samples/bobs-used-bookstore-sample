using System;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.Domain.Customers
{
    public class Customer
    {
        [Key] public string Customer_Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Phone { get; set; }
    }
}