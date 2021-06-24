using System.ComponentModel.DataAnnotations;

namespace BookstoreBackend.Models.Customer
{
    public class Address
    {
        /*
         * Address Model
         * Object reference indicates relationship with another Table
         */
        [Key]
        public long Address_Id { get; set; }

        public bool IsPrimary { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public long ZipCode { get; set; }

        // Many to One Relationship with Customer Table
        public Customer Customer { get; set; }

    }
}
