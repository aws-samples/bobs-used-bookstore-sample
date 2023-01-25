using Bookstore.Domain.Customers;
using Bookstore.Web.ViewModel.Address;

namespace Bookstore.Web.Mappers
{
    public static class AddressCreateViewModelToAddress
    {
        public static Address ToAddress(this AddressCreateUpdateViewModel model)
        {
            return new Address
            {
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                City = model.City,
                State = model.State,
                Country = model.Country,
                ZipCode = model.ZipCode
            };
        }

        public static Address ToAddress(this AddressCreateUpdateViewModel model, Address address)
        {
            address.AddressLine1 = model.AddressLine1;
            address.AddressLine2 = model.AddressLine2;
            address.City = model.City;
            address.State = model.State;
            address.Country = model.Country;
            address.ZipCode = model.ZipCode;

            return address;
        }
    }
}
