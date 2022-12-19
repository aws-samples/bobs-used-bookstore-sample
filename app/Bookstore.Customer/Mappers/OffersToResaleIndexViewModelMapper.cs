using Bookstore.Customer.ViewModel.Resale;
using Bookstore.Domain;
using Bookstore.Domain.Offers;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Customer.Mappers
{
    public static class OffersToResaleIndexViewModelMapper
    {
        public static ResaleIndexViewModel ToResaleIndexViewModel(this IEnumerable<Offer> offers)
        {
            return new ResaleIndexViewModel
            {
                Items = offers.Select(x => new ResaleIndexItemViewModel
                {
                    Author = x.Author,
                    BookName = x.BookName,
                    BookType = x.BookType.Text,
                    Condition = x.Condition.Text,
                    Genre = x.Genre.Text,
                    ISBN = x.ISBN,
                    OfferStatus = x.OfferStatus.GetDescription(),
                    Price = x.BookPrice,
                    Publisher = x.Publisher.Text
                }).ToList()
            };
        }
    }
}