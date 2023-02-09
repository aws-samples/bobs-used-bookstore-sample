namespace Bookstore.Domain.ReferenceData
{
    public interface IReferenceDataRepository
    {
        protected internal Task<IEnumerable<ReferenceDataItem>> FullListAsync();

        protected internal Task<IPaginatedList<ReferenceDataItem>> ListAsync(ReferenceDataFilters filters, int pageIndex, int pageSize);

        protected internal Task<ReferenceDataItem> GetAsync(int id);

        protected internal Task AddAsync(ReferenceDataItem item);

        protected internal Task SaveChangesAsync();
    }
}
