using Bookstore.Domain.Offers;
using Bookstore.Services.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Bookstore.Web.Areas.Admin.Models.Offers
{
    public class OfferIndexViewModel : PaginatedViewModel
    {
        public List<OfferIndexItemViewModel> Items { get; set; } = new List<OfferIndexItemViewModel>();

        public OfferFilters Filters { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> BookConditions { get; set; } = new List<SelectListItem>();
    }

    public class OfferIndexItemViewModel
    {
        public int OfferId { get; set; }

        public string BookName { get; set; }

        public string CustomerName { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }

        public OfferStatus OfferStatus { get; set; }

        public DateTime OfferDate { get; internal set; }

        public decimal OfferPrice { get; internal set; }

        public string Condition { get; internal set; }
    }
}
