using Bookstore.Domain;
using Bookstore.Domain.Books;
using Bookstore.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        async Task IOrderRepository.AddAsync(Order order)
        {
            await dbContext.Order.AddAsync(order);
        }

        async Task<Order> IOrderRepository.GetAsync(int id)
        {
            return await dbContext.Order
                .Include(x => x.Customer)
                .Include(x => x.Address)
                .Include(x => x.OrderItems).ThenInclude(x => x.Book).ThenInclude(x => x.BookType)
                .Include(x => x.OrderItems).ThenInclude(x => x.Book).ThenInclude(x => x.Condition)
                .Include(x => x.OrderItems).ThenInclude(x => x.Book).ThenInclude(x => x.Genre)
                .Include(x => x.OrderItems).ThenInclude(x => x.Book).ThenInclude(x => x.Publisher)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        async Task<Order> IOrderRepository.GetAsync(int id, string sub)
        {
            return await dbContext.Order.SingleOrDefaultAsync(x => x.Id == id && x.Customer.Sub == sub);
        }

        async Task<IEnumerable<Book>> IOrderRepository.ListBestSellingBooksAsync(int count)
        {
            return await dbContext.OrderItem
                .GroupBy(x => x.BookId)
                .OrderByDescending(x => x.Count())
                .Select(x => x.First().Book)
                .Take(count)
                .ToListAsync();
        }

        async Task<OrderStatistics> IOrderRepository.GetStatisticsAsync()
        {
            var startOfMonth = DateTime.UtcNow.StartOfMonth();

            return await dbContext.Order
                .GroupBy(x => 1)
                .Select(x => new OrderStatistics
                {
                    PendingOrders = x.Count(y => y.OrderStatus == OrderStatus.Pending),
                    PastDueOrders = x.Count(y => y.OrderStatus == OrderStatus.Ordered && y.DeliveryDate < DateTime.UtcNow),
                    OrdersThisMonth = x.Count(y => y.CreatedOn >= startOfMonth),
                    OrdersTotal = x.Count()
                }).SingleOrDefaultAsync();
        }

        async Task<IPaginatedList<Order>> IOrderRepository.ListAsync(OrderFilters filters, int pageIndex, int pageSize)
        {
            var query = dbContext.Order.AsQueryable();

            if (filters.OrderStatusFilter.HasValue)
            {
                query = query.Where(x => x.OrderStatus == filters.OrderStatusFilter);
            }

            if (filters.OrderDateFromFilter.HasValue)
            {
                query = query.Where(x => x.CreatedOn >= filters.OrderDateFromFilter);
            }

            if (filters.OrderDateToFilter.HasValue)
            {
                query = query.Where(x => x.CreatedOn < filters.OrderDateToFilter.Value.OneSecondToMidnight());
            }

            query = query
                .Include(x => x.Customer)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Book);

            var result = new PaginatedList<Order>(query, pageIndex, pageSize);

            await result.PopulateAsync();

            return result;
        }

        async Task<IEnumerable<Order>> IOrderRepository.ListAsync(string sub)
        {
            return await dbContext.Order
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Book)
                .Where(x => x.Customer.Sub == sub)
                .ToListAsync();
        }

        async Task IOrderRepository.SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}