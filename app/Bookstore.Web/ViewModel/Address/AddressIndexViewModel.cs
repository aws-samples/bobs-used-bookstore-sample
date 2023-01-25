using System.Collections.Generic;

namespace Bookstore.Web.ViewModel.Address
{
    public class AddressIndexViewModel
    {
        public List<AddressIndexItemViewModel> Items { get; set; } = new List<AddressIndexItemViewModel>();
    }

    public class AddressIndexItemViewModel
    {
        public int Id { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }
    }
}
