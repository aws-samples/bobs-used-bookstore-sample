using System.ComponentModel.DataAnnotations;

namespace Bookstore.Web.ViewModel.Address
{
    public class AddressCreateUpdateViewModel
    {
        public AddressCreateUpdateViewModel() { }

        public AddressCreateUpdateViewModel(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public AddressCreateUpdateViewModel(Domain.Addresses.Address address, string returnUrl)
        {
            Id = address.Id;
            AddressLine1 = address.AddressLine1;
            AddressLine2 = address.AddressLine2;
            City = address.City;
            Country = address.Country;
            State = address.State;
            ZipCode = address.ZipCode;
            ReturnUrl = returnUrl;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "The Street Address field is required.")]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string ZipCode { get; set; }

        public string ReturnUrl { get; set; }
    }
}
