using Bookstore.Domain.Books;

namespace Bookstore.Domain.Orders
{
    public interface IOrderRepository
    {
        internal protected Task<Order> GetAsync(int id);

        internal protected Task<Order> GetAsync(int id, string sub);

        internal protected Task<IEnumerable<Book>> ListBestSellingBooksAsync(int count);

        internal protected Task<IPaginatedList<Order>> ListAsync(OrderFilters filters, int pageIndex = 1, int pageSize = 10);

        internal protected Task<IEnumerable<Order>> ListAsync(string sub);

        internal protected Task AddAsync(Order order);

        internal protected Task<OrderStatistics> GetStatisticsAsync();

        internal protected Task SaveChangesAsync();
    }
}
