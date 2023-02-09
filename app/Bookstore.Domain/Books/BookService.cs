namespace Bookstore.Domain.Books
{
    public interface IBookService
    {
        Task<Book> GetBookAsync(int id);

        Task<IPaginatedList<Book>> GetBooksAsync(BookFilters filters, int pageIndex, int pageSize);

        Task<IPaginatedList<Book>> GetBooksAsync(string searchString, string sortBy, int pageIndex, int pageSize);

        Task AddAsync(CreateBookDto createBookDto);

        Task UpdateAsync(UpdateBookDto updateBookDto);
    }

    public class BookService : IBookService
    {
        private readonly IFileService fileService;
        private readonly IBookRepository bookRepository;

        public BookService(IFileService fileService, IBookRepository bookRepository)
        {
            this.fileService = fileService;
            this.bookRepository = bookRepository;
        }

        public async Task<Book> GetBookAsync(int id)
        {
            return await bookRepository.GetAsync(id);
        }

        public async Task<IPaginatedList<Book>> GetBooksAsync(BookFilters filters, int pageIndex, int pageSize)
        {
            return await bookRepository.ListAsync(filters, pageIndex, pageSize);
        }

        public async Task<IPaginatedList<Book>> GetBooksAsync(string searchString, string sortBy, int pageIndex, int pageSize)
        {
            return await bookRepository.ListAsync(searchString, sortBy, pageIndex, pageSize);
        }

        public async Task AddAsync(CreateBookDto dto)
        {
            var book = new Book(
                dto.Name,
                dto.Author,
                dto.ISBN,
                dto.PublisherId,
                dto.BookTypeId,
                dto.GenreId,
                dto.ConditionId,
                dto.Price,
                dto.Quantity,
                dto.Year,
                dto.Summary);

            await UpdateImageAsync(book, dto.CoverImage, dto.CoverImageFileName);

            await bookRepository.AddAsync(book);

            await bookRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateBookDto dto)
        {
            var book = await bookRepository.GetAsync(dto.BookId);

            book.Name = dto.Name;
            book.Author = dto.Author;
            book.ISBN = dto.ISBN;
            book.PublisherId = dto.PublisherId;
            book.BookTypeId = dto.BookTypeId;
            book.GenreId = dto.GenreId;
            book.ConditionId = dto.ConditionId;
            book.Price = dto.Price;
            book.Quantity = dto.Quantity;
            book.Year = dto.Year;
            book.Summary = dto.Summary;

            await UpdateImageAsync(book, dto.CoverImage, dto.CoverImageFileName);

            book.UpdatedOn = DateTime.UtcNow;

            await bookRepository.UpdateAsync(book);

            await bookRepository.SaveChangesAsync();
        }

        private async Task UpdateImageAsync(Book book, Stream? coverImage, string? coverImageFilename)
        {
            var imageUrl = await fileService.SaveAsync(coverImage, coverImageFilename);

            if (coverImage != null)
            {
                await fileService.DeleteAsync(book.CoverImageUrl);
                book.CoverImageUrl = imageUrl;
            }
        }
    }
}