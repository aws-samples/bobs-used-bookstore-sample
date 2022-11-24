using Bookstore.Admin.ViewModel.Offers;
using Bookstore.Domain;
using Bookstore.Domain.Offers;
using System.Linq;

namespace Bookstore.Admin.Mappers.Offers
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
                    BookName= offer.BookName,
                    Author= offer.Author,
                    GenreName= offer.GenreName,
                    CustomerName = offer.Customer.FullName,
                    OfferStatus = offer.OfferStatus,
                    OfferDate = offer.CreatedOn,
                    OfferPrice = offer.BookPrice,
                    BookCondition = offer.ConditionName
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
