using Bookstore.Data.Repository.Interface;
using Bookstore.Domain;
using Bookstore.Domain.Offers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public interface IOfferService
    {
        PaginatedList<Offer> GetOffers(string userName, int index, int count);

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

        public PaginatedList<Offer> GetOffers(string userName, int index, int count)
        {
            return offerRepository.GetPaginated(pageIndex: index, pageSize: count, includeProperties: x => x.Customer);
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