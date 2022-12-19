using Bookstore.Data.Repository.Interface;
using Bookstore.Domain;
using Bookstore.Domain.Customers;
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

        IEnumerable<Offer> GetOffers(string sub);

        Task CreateOfferAsync(Offer offer, string sub);
    }

    public class OfferService : IOfferService
    {
        private readonly IGenericRepository<Offer> offerRepository;
        private readonly IGenericRepository<Customer> customerRepository;

        public OfferService(IGenericRepository<Offer> offerRepository, IGenericRepository<Customer> customerRepository)
        {
            this.offerRepository = offerRepository;
            this.customerRepository = customerRepository;
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

        public IEnumerable<Offer> GetOffers(string sub)
        {
            return offerRepository.Get2(x => x.Customer.Sub == sub, null, x => x.BookType, x => x.Genre, x => x.Condition, x => x.Publisher);
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

        public async Task CreateOfferAsync(Offer offer, string sub)
        {
            var customer = customerRepository.Get2(x => x.Sub == sub).Single();

            offer.Customer = customer;

            await offerRepository.AddAsync(offer);

            await offerRepository.SaveAsync();
        }
    }
}