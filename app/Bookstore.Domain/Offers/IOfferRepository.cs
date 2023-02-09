namespace Bookstore.Domain.Offers
{
    public interface IOfferRepository
    {
        protected internal Task<IPaginatedList<Offer>> ListAsync(OfferFilters filters, int pageIndex, int pageSize);

        protected internal Task<IEnumerable<Offer>> ListAsync(string sub);

        protected internal Task<Offer> GetAsync(int id);

        protected internal Task AddAsync(Offer offer);

        protected internal Task SaveChangesAsync();
    }
}
