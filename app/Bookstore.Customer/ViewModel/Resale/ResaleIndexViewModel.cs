using System.Collections.Generic;

namespace Bookstore.Customer.ViewModel.Resale
{
    public class ResaleIndexViewModel
    {
        public List<ResaleIndexItemViewModel> Items { get; set; } = new List<ResaleIndexItemViewModel>();
    }

    public class ResaleIndexItemViewModel
    {
        public string BookName { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }

        public string Publisher { get; set; }

        public string BookType { get; set; }

        public string ISBN { get; set; }

        public string Condition { get; set; }

        public decimal Price { get; set; }

        public string OfferStatus { get; set; }
    }
}
