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
using Amazon.Polly;
using Amazon.Polly.Model;
using BOBS_Backend.Models.Order;
using BOBS_Backend.ViewModel.ManageInventory;

namespace BOBS_Backend
{
    public interface IInventory
    {

        public void SaveBook(Book book );

        public PagedSearchViewModel GetAllBooks(int pagenum, string style , string SortBy , string ascdesc , string pagination);

        public BookDetails GetBookByID(long Id);

        public void SavePrice(Price price);

        public void SavePublisherDetails(Publisher publisher);

      
        public int AddPublishers(BOBS_Backend.Models.Book.Publisher publishers);

        public int AddGenres(BOBS_Backend.Models.Book.Genre genres);

        public int AddBookTypes(BOBS_Backend.Models.Book.Type booktype);

        public int AddBookConditions(BOBS_Backend.Models.Book.Condition bookcondition);

        public List<BOBS_Backend.Models.Book.Type> GetTypes();

        public List<BOBS_Backend.Models.Book.Publisher> GetAllPublishers();

        public List<BOBS_Backend.Models.Book.Genre> GetGenres();

        public List<BOBS_Backend.Models.Book.Condition> GetConditions();

        public BookDetails GetBookDetails(long bookid, long priceid);

        public int AddToTables(ViewModel.BooksViewModel bookview);

        public IEnumerable<BookDetails> GetDetails(long BookId);

        public PagedSearchViewModel SearchBeta(string searchby, string Searchfilter , string style , string SortBy , int pagnum , string ascdesc ,  string pagination);

        public List<string> GetFormatsOfTheSelectedBook(string bookname);

        public List<string> GetConditionsOfTheSelectedBook(string bookname);

        public List<BookDetails> GetRelevantBooks(string Bookname , string type , string condition_chosen);

        public List<Dictionary<string, int>> DashBoard();


        public void PushDetails(BookDetails details);

        public List<string> autosuggest(string input);

        public BookDetails UpdateDetails(int Id, string Condition);



    }
}