using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOBS_Backend.DataModel;
using BOBS_Backend.Models;
using BOBS_Backend.Models.Book;
using Microsoft.AspNetCore.Http;


namespace BOBS_Backend
{
    public interface IInventory
    {

        public void SaveBook(Book book);

        public IEnumerable<BookDetails> GetAllBooks();

        public Book GetBookByID(long Id);

        public void SavePrice(Price price);

        public void SavePublisherDetails(Publisher publisher);

        public Task<string> UploadtoS3(IFormFile file);

        public Task<bool> IsImageSafe(string bucket, string key);

        public Task<bool> IsBook(string bucket, string key);

        public IEnumerable<BookDetails> GetRequestedBooks(string BookName, Publisher Publisher, Condition BookCondition, BOBS_Backend.Models.Book.Type type);

        public void AddPublishers(BOBS_Backend.Models.Book.Publisher publishers);
        public void AddGenres(BOBS_Backend.Models.Book.Genre genres);

        public void AddBookTypes(BOBS_Backend.Models.Book.Type booktype);

        public List<string> GetTypes();
    }
}