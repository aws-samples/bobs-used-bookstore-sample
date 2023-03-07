namespace Bookstore.Domain.Books
{
    public  interface IBookRepository
    {
        protected internal Task<Book> GetAsync(int id);

        protected internal Task<IPaginatedList<Book>> ListAsync(BookFilters filters, int pageIndex, int pageSize);

        protected internal Task<IPaginatedList<Book>> ListAsync(string searchString, string sortBy, int pageIndex, int pageSize);

        protected internal Task AddAsync(Book book);

        protected internal Task UpdateAsync(Book book);

        Task SaveChangesAsync();

        Task<BookStatistics> GetStatisticsAsync();
    }
}
