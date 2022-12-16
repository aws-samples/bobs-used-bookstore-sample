using Bookstore.Customer.ViewModel.Checkout;
using Bookstore.Domain.Carts;
using Bookstore.Domain.Customers;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Customer.Mappers
{
    public static class ShoppingCartToCheckoutIndexViewModel
    {
        public static CheckoutIndexViewModel ToCheckoutIndexViewModel(this ShoppingCart shoppingCart, IEnumerable<Address> addresses)
        {
            var result = new CheckoutIndexViewModel();

            result.Addresses =  addresses.Select(x => new CheckoutAddressViewModel
            {
                Id = x.Id,
                AddressLine1 = x.AddressLine1,
                AddressLine2 = x.AddressLine2,
                City = x.City,
                Country = x.Country,
                IsPrimary = x.IsPrimary,
                State = x.State,
                ZipCode = x.ZipCode
            }).ToList();

            result.ShoppingCartItems = shoppingCart.Items.Select(x => new CheckoutItemViewModel
            {
                BookName = x.Book.Name,
                ImageUrl = x.Book.FrontImageUrl,
                Price = x.Book.Price,
                OutOfStock = x.Book.Quantity <= 0
            }).ToList();

            result.Total = shoppingCart.GetSubTotal(ShoppingCartTotalType.ExcludeOutOfStockItems);

            result.SelectedAddressId = result.Addresses.Count > 0 ? result.Addresses.First().Id : 0;

            return result;
        }
    }
}
