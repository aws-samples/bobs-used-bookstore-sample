using Bookstore.Data;
using Bookstore.Domain;
using Bookstore.Domain.Books;
using Bookstore.Services;
using Bookstore.Services.Filters;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services
{
    public interface IInventoryService
    {
        Book GetBook(int id);

        PaginatedList<Book> GetBooks(InventoryFilters filters, int pageIndex, int pageSize);

        PaginatedList<Book> GetBooks(string searchString, string sortBy, int pageIndex, int pageSize);

        Task SaveAsync(Book book, IFormFile frontPhoto, IFormFile backPhoto, IFormFile leftPhoto, IFormFile rightPhoto, string userName);
    }

    public class InventoryService : IInventoryService
    {
        private readonly IFileService fileUploadService;
        private readonly IGenericRepository<Book> bookRepository;

        public InventoryService(IFileService fileUploadService, IGenericRepository<Book> bookRepository)
        {
            this.fileUploadService = fileUploadService;
            this.bookRepository = bookRepository;
        }

        public Book GetBook(int id)
        {
            return bookRepository.Get(x => x.Id == id, null, x => x.Genre, y => y.Publisher, x => x.BookType, x => x.Condition).SingleOrDefault();
        }

        public PaginatedList<Book> GetBooks(InventoryFilters filters, int pageIndex, int pageSize)
        {
            var filterExpressions = new List<Expression<Func<Book, bool>>>();

            if (!string.IsNullOrWhiteSpace(filters.Name))
            {
                filterExpressions.Add(x => x.Name.Contains(filters.Name));
            }

            if (!string.IsNullOrWhiteSpace(filters.Author))
            {
                filterExpressions.Add(x => x.Author.Contains(filters.Author));
            }

            if (filters.ConditionId.HasValue)
            {
                filterExpressions.Add(x => x.ConditionId == filters.ConditionId);
            }

            if (filters.BookTypeId.HasValue)
            {
                filterExpressions.Add(x => x.BookTypeId == filters.BookTypeId);
            }

            if (filters.GenreId.HasValue)
            {
                filterExpressions.Add(x => x.GenreId == filters.GenreId);
            }

            if (filters.PublisherId.HasValue)
            {
                filterExpressions.Add(x => x.PublisherId == filters.PublisherId);
            }

            return bookRepository
                .GetPaginated(filterExpressions, y => y.OrderBy(x => x.CreatedOn), pageIndex, pageSize, x => x.Genre, y => y.Publisher, x => x.BookType, x => x.Condition);
        }

        public PaginatedList<Book> GetBooks(string searchString, string sortBy, int pageIndex, int pageSize)
        {
            var sortByExpressions = new Dictionary<string, Func<IQueryable<Book>, IOrderedQueryable<Book>>>
            {
                { "Name", x => x.OrderBy(y => y.Name) },
                { "PriceAsc", x => x.OrderBy(y => y.Price) },
                { "PriceDesc", x => x.OrderByDescending(y => y.Price) }
            };

            Expression<Func<Book, bool>> filter = null;

            if (!string.IsNullOrWhiteSpace(searchString)) 
            {
                filter = b => b.Name.Contains(searchString) ||
                               b.Genre.Text.Contains(searchString) ||
                               b.BookType.Text.Contains(searchString) ||
                               b.ISBN.Contains(searchString) ||
                               b.Publisher.Text.Contains(searchString);
            };

            return bookRepository.GetPaginated(filter,
                                               sortByExpressions[sortBy],
                                               pageIndex,
                                               pageSize,
                                               book => book.Genre, book => book.BookType, book => book.Publisher);
        }

        public async Task SaveAsync(Book book, IFormFile frontImage, IFormFile backImage, IFormFile leftImage, IFormFile rightImage, string userName)
        {
            await UpdateImagesAsync(book, frontImage, backImage, leftImage, rightImage);

            if (book.IsNewEntity()) book.CreatedBy = userName;

            book.UpdatedOn = DateTime.UtcNow;

            bookRepository.AddOrUpdate(book);

            await bookRepository.SaveAsync();
        }

        private async Task UpdateImagesAsync(Book book, IFormFile frontImage, IFormFile backImage, IFormFile leftImage, IFormFile rightImage)
        {
            var frontImageUploadTask = fileUploadService.SaveAsync(frontImage);
            var backImageUploadTask = fileUploadService.SaveAsync(backImage);
            var leftImageUploadTask = fileUploadService.SaveAsync(leftImage);
            var rightImageUploadTask = fileUploadService.SaveAsync(rightImage);

            await Task.WhenAll(frontImageUploadTask, backImageUploadTask, leftImageUploadTask, rightImageUploadTask);

            if (frontImage != null)
            {
                await fileUploadService.DeleteAsync(book.FrontImageUrl);
                book.FrontImageUrl = frontImageUploadTask.Result;
            }

            if (backImage != null)
            {
                await fileUploadService.DeleteAsync(book.BackImageUrl);
                book.BackImageUrl = backImageUploadTask.Result;
            }

            if (leftImage != null)
            {
                await fileUploadService.DeleteAsync(book.LeftImageUrl);
                book.LeftImageUrl = leftImageUploadTask.Result;
            }

            if (rightImage != null)
            {
                await fileUploadService.DeleteAsync(book.RightImageUrl);
                book.RightImageUrl = rightImageUploadTask.Result;
            }
        }
    }
}