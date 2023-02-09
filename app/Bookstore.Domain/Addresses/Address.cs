using Bookstore.Domain.Customers;

namespace Bookstore.Domain.Addresses
{
    public class Address : Entity
    {
        private Address() { }

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

        public string AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public bool IsActive { get; set; } = true;
    }
}