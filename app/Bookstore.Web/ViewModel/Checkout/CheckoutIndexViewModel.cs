using Bookstore.Domain.Carts;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Web.ViewModel.Checkout
{
    public class CheckoutIndexViewModel
    {
        public decimal Total { get; set; }

        public List<CheckoutAddressViewModel> Addresses { get; set; } = new List<CheckoutAddressViewModel>();

        public int SelectedAddressId { get; set; }

        public List<CheckoutItemViewModel> ShoppingCartItems { get; set; } = new List<CheckoutItemViewModel>();

        public CheckoutIndexViewModel() { }

        public CheckoutIndexViewModel(Domain.Carts.ShoppingCart shoppingCart, IEnumerable<Domain.Addresses.Address> addresses)
        {
            Addresses = addresses.Select(x => new CheckoutAddressViewModel
            {
                Id = x.Id,
                AddressLine1 = x.AddressLine1,
                AddressLine2 = x.AddressLine2,
                City = x.City,
                Country = x.Country,
                State = x.State,
                ZipCode = x.ZipCode
            }).ToList();

            ShoppingCartItems = shoppingCart.GetShoppingCartItems(ShoppingCartItemFilter.IncludeOutOfStockItems).Select(x => new CheckoutItemViewModel
            {
                BookName = x.Book.Name,
                ImageUrl = x.Book.CoverImageUrl,
                Price = x.Book.Price,
                OutOfStock = x.Book.Quantity <= 0
            }).ToList();

            Total = shoppingCart.GetSubTotal(ShoppingCartItemFilter.ExcludeOutOfStockItems);

            SelectedAddressId = Addresses.Count > 0 ? Addresses.First().Id : 0;
        }
    }

    public class CheckoutAddressViewModel
    {
        public int Id { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public bool IsPrimary { get; set; }
    }

    public class CheckoutItemViewModel
    {
        public string BookName { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public bool OutOfStock { get; set; }
    }
}
