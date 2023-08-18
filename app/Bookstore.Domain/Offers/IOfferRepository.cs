using Bookstore.Domain.Orders;

namespace Bookstore.Domain.Offers
{
    public interface IOfferRepository
    {
        internal protected Task<IPaginatedList<Offer>> ListAsync(OfferFilters filters, int pageIndex, int pageSize);

        internal protected Task<IEnumerable<Offer>> ListAsync(string sub);

        internal protected Task<Offer> GetAsync(int id);

        internal protected Task AddAsync(Offer offer);

        internal protected Task SaveChangesAsync();

        Task<OfferStatistics> GetStatisticsAsync();
    }
}
