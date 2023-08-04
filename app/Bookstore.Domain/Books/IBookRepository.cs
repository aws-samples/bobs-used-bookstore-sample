namespace Bookstore.Domain.Books
{
    public interface IBookRepository
    {
        internal protected Task<Book> GetAsync(int id);

        internal protected Task<IPaginatedList<Book>> ListAsync(BookFilters filters, int pageIndex, int pageSize);

        internal protected Task<IPaginatedList<Book>> ListAsync(string searchString, string sortBy, int pageIndex, int pageSize);

        internal protected Task AddAsync(Book book);

        internal protected Task UpdateAsync(Book book);

        Task SaveChangesAsync();

        Task<BookStatistics> GetStatisticsAsync();
    }
}
