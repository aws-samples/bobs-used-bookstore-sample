using Bookstore.Domain;
using Bookstore.Domain.Offers;
using Bookstore.Web.Areas.Admin.Models.Offers;
using System.Linq;

namespace Bookstore.Web.Areas.Admin.Mappers.Offers
{
    public static class PaginatedOfferListToOfferIndexViewModelMapper
    {
        public static OfferIndexViewModel ToOfferIndexViewModel(this PaginatedList<Offer> offers)
        {
            var result = new OfferIndexViewModel();

            foreach (var offer in offers)
            {
                result.Items.Add(new OfferIndexItemViewModel
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

            result.PageIndex = offers.PageIndex;
            result.PageSize = offers.Count;
            result.PageCount = offers.TotalPages;
            result.HasNextPage = offers.HasNextPage;
            result.HasPreviousPage = offers.HasPreviousPage;
            result.PaginationButtons = offers.GetPageList(5).ToList();

            return result;
        }
    }
}
