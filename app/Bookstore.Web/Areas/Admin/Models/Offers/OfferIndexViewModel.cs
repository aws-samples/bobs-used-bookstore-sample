using Bookstore.Domain;
using Bookstore.Domain.Offers;
using Bookstore.Domain.ReferenceData;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Web.Areas.Admin.Models.Offers
{
    public class OfferIndexViewModel : PaginatedViewModel
    {
        public OfferIndexViewModel(IPaginatedList<Offer> offers, IEnumerable<ReferenceDataItem> referenceData)
        {
            foreach (var offer in offers)
            {
                Items.Add(new OfferIndexItemViewModel
                {
                    OfferId = offer.Id,
                    BookName = offer.BookName,
                    Author = offer.Author,
                    Genre = offer.Genre.Text,
                    CustomerName = offer.Customer.FullName,
                    OfferStatus = offer.OfferStatus,
                    OfferDate = offer.CreatedOn,
                    OfferPrice = offer.BookPrice,
                    Condition = offer.Condition.Text
                });
            }

            PageIndex = offers.PageIndex;
            PageSize = offers.Count;
            PageCount = offers.TotalPages;
            HasNextPage = offers.HasNextPage;
            HasPreviousPage = offers.HasPreviousPage;
            PaginationButtons = offers.GetPageList(5).ToList();

            Genres = referenceData.Where(x => x.DataType == ReferenceDataType.Genre).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            BookConditions = referenceData.Where(x => x.DataType == ReferenceDataType.Condition).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
        }

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
