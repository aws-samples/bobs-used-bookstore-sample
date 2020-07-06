using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BOBS_Backend.DataModel;
using BOBS_Backend.Models;
using BOBS_Backend.Models.Book;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;



namespace BOBS_Backend
{
    public interface IInventory
    {

        public void SaveBook(Book book );

        public IEnumerable<BookDetails> GetAllBooks();

        public Book GetBookByID(long Id);

        public void SavePrice(Price price);

        public void SavePublisherDetails(Publisher publisher);

        public Task<string> UploadtoS3(IFormFile file);

        public Task<bool> IsImageSafe(string bucket, string key);

        public Task<bool> IsBook(string bucket, string key);

        public IEnumerable<BookDetails> GetRequestedBooks(string searchby, string Searchfilter);

        public int AddPublishers(BOBS_Backend.Models.Book.Publisher publishers);

        public int AddGenres(BOBS_Backend.Models.Book.Genre genres);

        public int AddBookTypes(BOBS_Backend.Models.Book.Type booktype);

        public int AddBookConditions(BOBS_Backend.Models.Book.Condition bookcondition);

        public List<BOBS_Backend.Models.Book.Type> GetTypes();

        public List<BOBS_Backend.Models.Book.Publisher> GetAllPublishers();

        public List<BOBS_Backend.Models.Book.Genre> GetGenres();

        public List<BOBS_Backend.Models.Book.Condition> GetConditions();

        public IImageEncoder selectEncoder(string extension);

        public  Task<Stream> ResizeImage(IFormFile file, string fileExt);

        public BookDetails GetBookDetails(long bookid, long priceid);


    }
}