using Bookstore.Domain;
using Bookstore.Domain.Books;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BookRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        async Task<Book> IBookRepository.GetAsync(int id)
        {
            return await dbContext.Books
                .Include(x => x.Genre)
                .Include(y => y.Publisher)
                .Include(x => x.BookType)
                .Include(x => x.Condition)
                .SingleAsync(x => x.Id == id);
        }

        async Task<IPaginatedList<Book>> IBookRepository.ListAsync(BookFilters filters, int pageIndex, int pageSize)
        {
            var query = dbContext.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.Name))
            {
                query = query.Where(x => x.Name.Contains(filters.Name));
            }

            if (!string.IsNullOrWhiteSpace(filters.Author))
            {
                query = query.Where(x => x.Author.Contains(filters.Author));
            }

            if (filters.ConditionId.HasValue)
            {
                query = query.Where(x => x.ConditionId == filters.ConditionId);
            }

            if (filters.BookTypeId.HasValue)
            {
                query = query.Where(x => x.BookTypeId == filters.BookTypeId);
            }

            if (filters.GenreId.HasValue)
            {
                query = query.Where(x => x.GenreId == filters.GenreId);
            }

            if (filters.PublisherId.HasValue)
            {
                query = query.Where(x => x.PublisherId == filters.PublisherId);
            }

            if (filters.LowStock)
            {
                query = query.Where(x => x.Quantity <= Book.LowBookThreshold);
            }

            query = query
                .Include(x => x.Genre)
                .Include(x => x.Publisher)
                .Include(x => x.BookType)
                .Include(x => x.Condition);

            var result = new PaginatedList<Book>(query, pageIndex, pageSize);

            await result.PopulateAsync();

            return result;
        }

        public async Task<IPaginatedList<Book>> ListAsync(string searchString, string sortBy, int pageIndex, int pageSize)
        {
            var query = dbContext.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x => x.Name.Contains(searchString) ||
                                         x.Genre.Text.Contains(searchString) ||
                                         x.BookType.Text.Contains(searchString) ||
                                         x.ISBN.Contains(searchString) ||
                                         x.Publisher.Text.Contains(searchString));
            };

            query = sortBy switch
            {
                "Name" => query.OrderBy(x => x.Name),
                "PriceAsc" => query.OrderBy(x => x.Price),
                "PriceDesc" => query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x => x.Name),
            };

            var result = new PaginatedList<Book>(query, pageIndex, pageSize);

            await result.PopulateAsync();

            return result;
        }

        async Task IBookRepository.AddAsync(Book book)
        {
            await dbContext.AddAsync(book);
        }

        async Task IBookRepository.UpdateAsync(Book book)
        {
            var existing = await dbContext.Books.FindAsync(book.Id);

            dbContext.Entry(existing).CurrentValues.SetValues(book);

            if (string.IsNullOrWhiteSpace(book.CoverImageUrl))
            {
                dbContext.Entry(existing).Property(x => x.CoverImageUrl).IsModified = false;
            }
        }

        async Task IBookRepository.SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        async Task<BookStatistics> IBookRepository.GetStatisticsAsync()
        {
            return await dbContext.Books
                .GroupBy(x => 1)
                .Select(x => new BookStatistics
                {
                    LowStock = x.Count(y => y.Quantity > 0 && y.Quantity <= Book.LowBookThreshold),
                    OutOfStock = x.Count(y => y.Quantity == 0),
                    StockTotal = x.Count()
                }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> FindRelevantBooks()
        {
            var books = await dbContext.Books
                .OrderBy(b => b.Price)
                .ToListAsync();

            var filteredbooks = new List<Book>();

            foreach (var book in books)
            {
                if (book.GenreId == 13)
                {
                    filteredbooks.Add(book);
                }

                if (filteredbooks.Count == 3)
                {
                    break;
                }
            }
            filteredbooks = filteredbooks.OrderBy(x => x.Name).ToList();
            return filteredbooks;
        }
    }
}
