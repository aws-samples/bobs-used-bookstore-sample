using Bookstore.Domain;
using Bookstore.Domain.Offers;
using Bookstore.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Data.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OfferRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<OfferStatistics> GetStatisticsAsync()
        {
            var startOfMonth = DateTime.UtcNow.StartOfMonth();

            return await dbContext.Offer
                .GroupBy(x => 1)
                .Select(x => new OfferStatistics
                {
                    PendingOffers = x.Count(y => y.OfferStatus == OfferStatus.PendingApproval),
                    OffersThisMonth = x.Count(y => y.CreatedOn >= startOfMonth),
                    OffersTotal = x.Count()
                }).SingleOrDefaultAsync();
        }

        async Task IOfferRepository.AddAsync(Offer offer)
        {
            await dbContext.Offer.AddAsync(offer);
        }

        Task<Offer> IOfferRepository.GetAsync(int id)
        {
            return dbContext.Offer.Include(x => x.Customer).SingleOrDefaultAsync(x => x.Id == id);
        }

        async Task<IPaginatedList<Offer>> IOfferRepository.ListAsync(OfferFilters filters, int pageIndex, int pageSize)
        {
            var query = dbContext.Offer.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.Author))
            {
                query = query.Where(x => x.Author.Contains(filters.Author));
            }

            if (!string.IsNullOrWhiteSpace(filters.BookName))
            {
                query = query.Where(x => x.BookName.Contains(filters.BookName));
            }

            if (filters.ConditionId.HasValue)
            {
                query = query.Where(x => x.ConditionId == filters.ConditionId);
            }

            if (filters.GenreId.HasValue)
            {
                query = query.Where(x => x.GenreId == filters.GenreId);
            }

            if (filters.OfferStatus.HasValue)
            {
                query = query.Where(x => x.OfferStatus == filters.OfferStatus);
            }

            query = query.Include(x => x.Customer);

            var result = new PaginatedList<Offer>(query, pageIndex, pageSize);

            await result.PopulateAsync();

            return result;
        }

        async Task<IEnumerable<Offer>> IOfferRepository.ListAsync(string sub)
        {
            return await dbContext.Offer
                .Include(x => x.BookType)
                .Include(x => x.Genre)
                .Include(x => x.Condition)
                .Include(x => x.Publisher)
                .Where(x => x.Customer.Sub == sub)
                .ToListAsync();
        }

        async Task IOfferRepository.SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}