using Bookstore.Customer.ViewModel.Address;
using Bookstore.Domain.Customers;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Customer.Mappers
{
    public static class AddressesToAddressIndexViewModel
    {
        public static AddressIndexViewModel ToAddressIndexViewModel(this IEnumerable<Address> addresses)
        {
            return new AddressIndexViewModel
            {
                Items = addresses.Select(x => new AddressIndexItemViewModel
                {
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    City = x.City,
                    Country = x.Country,
                    Id = x.Id,
                    State = x.State,
                    ZipCode = x.ZipCode
                }).ToList()
            };
        }
    }
}