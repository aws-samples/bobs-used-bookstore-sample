using Bookstore.Domain.Offers;
using Bookstore.Web.ViewModel.Resale;

namespace Bookstore.Web.Mappers
{
    public static class OfferCreateViewModelToOfferMapper
    {
        public static Offer ToOffer(this ResaleCreateViewModel model)
        {
            return new Offer
            {
                Author = model.Author,
                BookName = model.BookName,
                BookPrice = model.BookPrice,
                BookTypeId = model.SelectedBookTypeId,
                ConditionId = model.SelectedConditionId,
                GenreId = model.SelectedGenreId,
                ISBN = model.ISBN,
                PublisherId = model.SelectedPublisherId
            };
        }
    }
}