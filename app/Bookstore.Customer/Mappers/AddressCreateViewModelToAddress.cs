using Bookstore.Customer.ViewModel.Addresses;
using Bookstore.Domain.Customers;

namespace Bookstore.Customer.Mappers
{
    public static class AddressCreateViewModelToAddress
    {
        public static Address ToAddress(this AddressCreateViewModel model, string customerId)
        {
            return new Address
            {
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                City = model.City,
                State = model.State,
                Country = model.Country,
                ZipCode = model.ZipCode,
                CustomerId = customerId
            };
        }
    }
}
