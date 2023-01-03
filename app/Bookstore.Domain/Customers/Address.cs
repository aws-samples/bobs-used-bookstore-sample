namespace Bookstore.Domain.Customers
{
    public class Address : Entity
    {
        public string AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}