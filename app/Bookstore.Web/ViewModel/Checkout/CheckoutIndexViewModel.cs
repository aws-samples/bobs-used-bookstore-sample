using System.Collections.Generic;
namespace Bookstore.Web.ViewModel.Checkout
{
    public class CheckoutIndexViewModel
    {
        public decimal Total { get; set; }

        public List<CheckoutAddressViewModel> Addresses { get; set; } = new List<CheckoutAddressViewModel>();

        public int SelectedAddressId { get; set; }

        public List<CheckoutItemViewModel> ShoppingCartItems { get; set; } = new List<CheckoutItemViewModel>();
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
