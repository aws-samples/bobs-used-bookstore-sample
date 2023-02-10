namespace Bookstore.Domain.Orders
{
    public interface IOrderRepository
    {
        protected internal Task<Order> GetAsync(int id);

        protected internal Task<Order> GetAsync(int id, string sub);

        protected internal Task<IPaginatedList<Order>> ListAsync(OrderFilters filters, int pageIndex = 1, int pageSize = 10);

        protected internal Task<IEnumerable<Order>> ListAsync(string sub);

        protected internal Task AddAsync(Order order);

        protected internal Task<OrderStatistics> GetStatisticsAsync();

        protected internal Task SaveChangesAsync();
    }
}
