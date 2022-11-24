using Bookstore.Domain.Offers;
using System;
using System.Collections.Generic;

namespace Bookstore.Admin.ViewModel.Offers
{
    public class OfferIndexViewModel : PaginatedViewModel
    {
        public List<OfferIndexItemViewModel> Items { get; set; } = new List<OfferIndexItemViewModel>();
    }

    public class OfferIndexItemViewModel
    {
        public int OfferId { get; set; }

        public string BookName { get; set; }

        public string CustomerName { get; set; }

        public string Author { get; set; }

        public string GenreName { get; set; }

        public OfferStatus OfferStatus { get; set; }

        public DateTime OfferDate { get; internal set; }

        public decimal OfferPrice { get; internal set; }

        public string BookCondition { get; internal set; }
    }
}
