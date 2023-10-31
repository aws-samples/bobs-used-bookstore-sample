using Bookstore.Domain.Orders;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Bookstore.Domain.Books
{
    public interface IBookService
    {
        Task<Book> GetBookAsync(int id);

        Task<IPaginatedList<Book>> GetBooksAsync(BookFilters filters, int pageIndex, int pageSize);

        Task<IPaginatedList<Book>> GetBooksAsync(string searchString, string sortBy, int pageIndex, int pageSize);

        Task<IEnumerable<Book>> ListBestSellingBooksAsync(int count);

        Task<BookStatistics> GetStatisticsAsync();

        Task<BookResult> AddAsync(CreateBookDto createBookDto);

        Task<BookResult> UpdateAsync(UpdateBookDto updateBookDto);
    }

    public class BookService : IBookService
    {
        private readonly IImageResizeService imageResizeService;
        private readonly IImageValidationService imageValidationService;
        private readonly IFileService fileService;
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<BookService> logger;

        public BookService(IImageResizeService imageResizeService, IImageValidationService imageValidationService, IFileService fileService, IBookRepository bookRepository, IOrderRepository orderRepository, ILoggerFactory logger)
        {
            this.imageResizeService = imageResizeService;
            this.imageValidationService = imageValidationService;
            this.fileService = fileService;
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.logger = logger.CreateLogger<BookService>();
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

        public async Task<IEnumerable<Book>> ListBestSellingBooksAsync(int count)
        {
            logger.LogDebug("BookService.ListBestSellingBooksAsync Invoked");
            return await orderRepository.ListBestSellingBooksAsync(count);
        }

        public async Task<BookStatistics> GetStatisticsAsync()
        {
            return (await bookRepository.GetStatisticsAsync()) ?? new BookStatistics();
        }

        public async Task<BookResult> AddAsync(CreateBookDto dto)
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

            await bookRepository.AddAsync(book);

            logger.LogInformation("Adding a new book to the inventory with the following properties: Name: {name}, Author: {author}, ISBN: {isbn}", book.Name, book.Author, book.ISBN);

            return await SaveAsync(book, dto.CoverImage, dto.CoverImageFileName);
        }

        public async Task<BookResult> UpdateAsync(UpdateBookDto dto)
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
            book.UpdatedOn = DateTime.UtcNow;

            await bookRepository.UpdateAsync(book);

            return await SaveAsync(book, dto.CoverImage, dto.CoverImageFileName);
        }

        private async Task<BookResult> SaveAsync(Book book, Stream? coverImage, string coverImageFileName)
        {
            var resizedCoverImage = await ResizeImageAsync(coverImage);

            var imageIsSafe = await imageValidationService.IsSafeAsync(coverImage);

            if (!imageIsSafe)
            {
                logger.LogError("The provided image for the book {name} failed the moderation checks.", book.Name);
                return new BookResult(false, "The image failed the safety check. Please try another image.");
            }

            await SaveImageAsync(book, resizedCoverImage, coverImageFileName);

            await bookRepository.SaveChangesAsync();

            return new BookResult(true, null);
        }

        private async Task<Stream?> ResizeImageAsync(Stream? coverImage)
        {
            if (coverImage == null) return null;

            return await imageResizeService.ResizeImageAsync(coverImage);
        }

        private async Task SaveImageAsync(Book book, Stream? coverImage, string? coverImageFilename)
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