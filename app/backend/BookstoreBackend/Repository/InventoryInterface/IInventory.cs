using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookstoreBackend.DataModel;
using BookstoreBackend.Models;
using BookstoreBackend.Models.Book;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using Amazon.Polly;
using Amazon.Polly.Model;
using BookstoreBackend.Models.Order;
using BookstoreBackend.ViewModel.ManageInventory;
using BookstoreBackend.ViewModel.SearchBooks;

namespace BookstoreBackend
{
    public interface IInventory
    {
        public SearchBookViewModel SearchBooks(string searchby, string searchfilter, string style, string SortBy, int pagenum, string ascdesc);

        public void SaveBook(Book book );

        public SearchBookViewModel GetAllBooks(int pagenum, string style, string SortBy, string ascdesc);

        public BookDetails GetBookByID(long Id);

        public void SavePrice(Price price);
      
        public int AddPublishers(BookstoreBackend.Models.Book.Publisher publishers);

        public int AddGenres(BookstoreBackend.Models.Book.Genre genres);

        public int AddBookTypes(BookstoreBackend.Models.Book.Type booktype);

        public int AddBookConditions(BookstoreBackend.Models.Book.Condition bookcondition);

        public List<BookstoreBackend.Models.Book.Type> GetTypes();

        public List<BookstoreBackend.Models.Book.Publisher> GetAllPublishers();

        public List<BookstoreBackend.Models.Book.Genre> GetGenres();

        public List<BookstoreBackend.Models.Book.Condition> GetConditions();

        public BookDetails GetBookDetails(long bookid, long priceid);

        public int AddToTables(ViewModel.BooksViewModel bookview);

        public IEnumerable<BookDetails> GetDetails(long BookId);


        public List<string> GetFormatsOfTheSelectedBook(string bookname);

        public List<string> GetConditionsOfTheSelectedBook(string bookname);

        public List<BookDetails> GetRelevantBooks(string Bookname , string type , string condition_chosen);

        public List<Dictionary<string, int>> DashBoard();


        public void PushDetails(BookDetails details);

        public List<string> autosuggest(string input);

        public BookDetails UpdateDetails(int Id, string Condition);

        public void EditPublisher(string Actual, string New);

        public void EditGenre(string Actual, string New);

        public void EditCondition(string Actual, string New);

        public void EditType(string Actual, string New);

        public List<string> ScreenInventory();



    }
}