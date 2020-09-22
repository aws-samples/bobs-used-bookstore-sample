using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.Customer
{
    public class Address
    {
        [Key]
        public long Address_Id { get; set; }

        public bool IsPrimary { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public long ZipCode { get; set; }

        public Customer Customer { get; set; }
    }
}
