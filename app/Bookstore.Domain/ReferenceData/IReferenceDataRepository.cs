namespace Bookstore.Domain.ReferenceData
{
    public interface IReferenceDataRepository
    {
        internal protected Task<IEnumerable<ReferenceDataItem>> FullListAsync();

        internal protected Task<IPaginatedList<ReferenceDataItem>> ListAsync(ReferenceDataFilters filters, int pageIndex, int pageSize);

        internal protected Task<ReferenceDataItem> GetAsync(int id);

        internal protected Task AddAsync(ReferenceDataItem item);

        internal protected Task SaveChangesAsync();
    }
}
