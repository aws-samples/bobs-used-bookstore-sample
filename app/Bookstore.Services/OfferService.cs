using Bookstore.Data.Repository.Interface;
using Bookstore.Domain;
using Bookstore.Domain.Offers;
using Bookstore.Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public interface IOfferService
    {
        PaginatedList<Offer> GetOffers(OfferFilters filters, int index, int count);

        Offer GetOffer(int id);

        Task SaveOfferAsync(Offer offer, string userName);
    }

    public class OfferService : IOfferService
    {
        private readonly IGenericRepository<Offer> offerRepository;

        public OfferService(IGenericRepository<Offer> offerRepository)
        {
            this.offerRepository = offerRepository;
        }

        public PaginatedList<Offer> GetOffers(OfferFilters filters, int index, int count)
        {
            var filterExpressions = new List<Expression<Func<Offer, bool>>>();

            if (!string.IsNullOrWhiteSpace(filters.Author))
            {
                filterExpressions.Add(x => x.Author.Contains(filters.Author));
            }

            if (!string.IsNullOrWhiteSpace(filters.BookName))
            {
                filterExpressions.Add(x => x.BookName.Contains(filters.BookName));
            }

            if (filters.ConditionId.HasValue)
            {
                filterExpressions.Add(x => x.ConditionId == filters.ConditionId);
            }

            if (filters.GenreId.HasValue)
            {
                filterExpressions.Add(x => x.GenreId == filters.GenreId);
            }

            if (filters.OfferStatus.HasValue)
            {
                filterExpressions.Add(x => x.OfferStatus == filters.OfferStatus);
            }

            return offerRepository.GetPaginated(filterExpressions, pageIndex: index, pageSize: count, includeProperties: x => x.Customer);
        }

        public Offer GetOffer(int id)
        {
            return offerRepository.Get2(x => x.Id == id, null, x => x.Customer).SingleOrDefault();
        }

        public async Task SaveOfferAsync(Offer offer, string userName)
        {
            if (offer.IsNewEntity()) offer.CreatedBy = userName;

            offer.UpdatedOn = DateTime.UtcNow;

            offerRepository.AddOrUpdate(offer);

            await offerRepository.SaveAsync();
        }
    }
}