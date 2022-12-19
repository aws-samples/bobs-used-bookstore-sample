using Bookstore.Customer.ViewModel.Resale;
using Bookstore.Domain.Offers;

namespace Bookstore.Customer.Mappers
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