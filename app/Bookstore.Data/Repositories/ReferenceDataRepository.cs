using Bookstore.Domain;
using Bookstore.Domain.ReferenceData;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Data.Repositories
{
    public class ReferenceDataRepository : IReferenceDataRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ReferenceDataRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        async Task IReferenceDataRepository.AddAsync(ReferenceDataItem item)
        {
            await dbContext.ReferenceData.AddAsync(item);
        }

        async Task<ReferenceDataItem> IReferenceDataRepository.GetAsync(int id)
        {
            return await dbContext.ReferenceData.FindAsync(id);
        }

        async Task<IEnumerable<ReferenceDataItem>> IReferenceDataRepository.FullListAsync()
        {
            return await dbContext.ReferenceData.ToListAsync();
        }

        async Task<IPaginatedList<ReferenceDataItem>> IReferenceDataRepository.ListAsync(ReferenceDataFilters filters, int pageIndex, int pageSize)
        {
            var query = dbContext.ReferenceData.AsQueryable();

            if (filters.ReferenceDataType.HasValue)
            {
                query = query.Where(x => x.DataType == filters.ReferenceDataType.Value);
            }

            var result = new PaginatedList<ReferenceDataItem>(query, pageIndex, pageSize);

            await result.PopulateAsync();

            return result;
        }

        async Task IReferenceDataRepository.SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}