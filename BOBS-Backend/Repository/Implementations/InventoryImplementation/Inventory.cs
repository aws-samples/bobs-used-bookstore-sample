using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Transfer;
using BOBS_Backend.Database;
using BOBS_Backend.DataModel;
using BOBS_Backend.Models;
using BOBS_Backend.Models.Book;
using BOBS_Backend.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.S3.Model.Internal.MarshallTransformations;
using BOBS_Backend.Models.Order;
using Amazon.Runtime.Internal.Transform;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Amazon.Translate.Model;
using BOBS_Backend.Repository.Implementations;
using BOBS_Backend.ViewModel.ManageInventory;
using System.Linq.Expressions;
using Type = System.Type;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BOBS_Backend
{
    /*
        * Inventory Repository contains all functions associated with managing and viewing Inventory
        */

    public class Inventory : IInventory
    {

        public DatabaseContext _context;
        private readonly int _booksPerPage = 15;
        private readonly IRekognitionNPollyRepository _RekognitionNPollyRepository;
        private readonly ILogger<Inventory> _logger;



        public Inventory(DatabaseContext context, IRekognitionNPollyRepository RekognitionNPollyRepository , ILogger<Inventory> logger)
        {
            _context = context;
            _RekognitionNPollyRepository = RekognitionNPollyRepository;
            _logger = logger;
        }

        public BookDetails GetBookByID(long BookId)
        {

            var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == BookId select new BookDetails {Summary = booke.Summary, ISBN = booke.ISBN ,Author = booke.Author, BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            return book[0];
        }

        /*
         *  function to add details to the Book table
         */
        public void SaveBook(Book book)
        {
            _logger.LogInformation("Posting details to Books table");
            _context.Book.Add(book);
            _context.SaveChanges();
        }

        /*
         *  function to add details to the Price table
         */
        public void SavePrice(Price price)
        {
            _logger.LogInformation("Posting details to Price table");
            _context.Price.Add(price);
            _context.SaveChanges();
        }

        /*
         *  function to add  details to the Publisher table
         */
        public void SavePublisherDetails(Publisher publisher)
        {
            _logger.LogInformation("Posting details to the Publisher table");
            var publishers = _context.Publisher.Find(publisher.Name);
            if (publishers == null)
            {
                _context.Publisher.Add(publisher);
                _context.SaveChanges();
            }
        }

        /*
         *  function to add  process search input and display paged output 
         */
       
        /*
         *  function to add  details to the Publisher table
         */

        public int AddPublishers(BOBS_Backend.Models.Book.Publisher publishers)
        {
            _logger.LogInformation("Posting details to the Publisher table");
            var publishName = _context.Publisher.Where(publisher => publisher.Name == publishers.Name).ToList();
            if (publishName.Count == 0)
            {
                _context.Publisher.Add(publishers);
                _context.SaveChanges();
                return 0;
            }
            return 1;
        }

        /*
         *  function to add  details to the Genres table
         */
        public int AddGenres(BOBS_Backend.Models.Book.Genre genres)
        {
            _logger.LogInformation("Posting details to the Genres table");
            var genreName = _context.Genre.Where(genre => genre.Name == genres.Name).ToList();
            if (genreName.Count == 0)
            {
                _context.Genre.Add(genres);
                _context.SaveChanges();
                return 0;
            }
            return 1;
        }

        /*
         *  function to add  details to the Types table
         */
        public int AddBookTypes(BOBS_Backend.Models.Book.Type booktype)
        {
            _logger.LogInformation("Posting details to the Types table");
            var typeName = _context.Type.Where(type => type.TypeName == booktype.TypeName).ToList();
            var typeId = _context.Type.Where(type => type.Type_Id == booktype.Type_Id).ToList();

            if (typeName.Count == 0)
            {
                _context.Type.Add(booktype);
                _context.SaveChanges();
                return 0;
            }
            return 1;
        }

        /*
         *  function to add  details to the Conditions table
         */
        public int AddBookConditions(BOBS_Backend.Models.Book.Condition bookcondition)
        {
            _logger.LogInformation("Posting details to the Conditions table");
            _context.Condition.Add(bookcondition);
            _context.SaveChanges();

            var conditionName = _context.Condition.Where(condition => condition.ConditionName == bookcondition.ConditionName).ToList();

            if (conditionName.Count == 0)
            {
                _context.Condition.Add(bookcondition);
                _context.SaveChanges();
                return 0;
            }
            return 1;

        }

        /*
         *  function to fetch all publisher details for dynamic display in the drop down list 
         */
        public List<BOBS_Backend.Models.Book.Publisher> GetAllPublishers()
        {
            var publishers = _context.Publisher.ToList();
            return publishers;
        }


        /*
        *  function to fetch all Genres details for dynamic display in the drop down list 
        */
        public List<BOBS_Backend.Models.Book.Genre> GetGenres()
        {

            var genres = _context.Genre.ToList();
            return genres;
        }

        /*
        *  function to fetch all Types details for dynamic display in the drop down list 
        */
        public List<BOBS_Backend.Models.Book.Type> GetTypes()
        {

            var typelist = _context.Type.ToList();
            return typelist;
        }

        /*
        *  function to fetch all Conditions for dynamic display in the drop down list 
        */
        public List<BOBS_Backend.Models.Book.Condition> GetConditions()
        {

            var conditions = _context.Condition.ToList();
            return conditions;
        }
        
         /*
         *  function to fetch Book details given its BookId and Condition t 
         */
        public BookDetails GetBookDetails(long bookid, long priceid)
        {

            var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == bookid && price.Price_Id == priceid select new BookDetails { BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            return booker[0];
        }

        /*
        *  function to fetch all process form inputs and add details to the database 
        */
        public int AddToTables(BooksViewModel bookview)
        {
            _logger.LogInformation("Processing and Posting data to tables and cloud ");

            string front_url = "", back_url = "", left_url = "", right_url = "", AudioBookUrl = "";
            if (bookview.FrontPhoto != null)
            {
                front_url = _RekognitionNPollyRepository.UploadtoS3(bookview.FrontPhoto , bookview.BookId , bookview.BookCondition).Result;
            }

            if (bookview.BackPhoto != null)
            {
                back_url = _RekognitionNPollyRepository.UploadtoS3(bookview.BackPhoto, bookview.BookId, bookview.BookCondition).Result;
            }

            if (bookview.LeftSidePhoto != null)
            {
                left_url = _RekognitionNPollyRepository.UploadtoS3(bookview.LeftSidePhoto, bookview.BookId, bookview.BookCondition).Result;
            }

            if (bookview.RightSidePhoto != null)
            {
                right_url = _RekognitionNPollyRepository.UploadtoS3(bookview.RightSidePhoto, bookview.BookId, bookview.BookCondition).Result;
            }

            if (_RekognitionNPollyRepository.IsContentViolation(front_url) == true || _RekognitionNPollyRepository.IsContentViolation(back_url) == true || _RekognitionNPollyRepository.IsContentViolation(left_url) == true || _RekognitionNPollyRepository.IsContentViolation(right_url) == true)
            {
                return 0;
            }

            if (bookview.Summary != null)
            {
                AudioBookUrl = _RekognitionNPollyRepository.GenerateAudioSummary(bookview.BookName, bookview.Summary, "fr-CA", VoiceId.Emma);
            }

            Book book = new Book();
            Price price = new Price();

            var publisherdata = _context.Publisher.Where(publisher => publisher.Name == bookview.PublisherName).ToList();
            var genredata = _context.Genre.Where(genre => genre.Name == bookview.genre).ToList();
            var typedata = _context.Type.Where(type => type.TypeName == bookview.BookType).ToList();
            var conditiondata = _context.Condition.Where(condition => condition.ConditionName == bookview.BookCondition).ToList();

            book.Name = bookview.BookName;
            book.Type = typedata[0];
            book.Genre = genredata[0];
            book.ISBN = bookview.ISBN;
            book.Publisher = publisherdata[0];
            book.Front_Url = front_url;
            book.Back_Url = back_url;
            book.Left_Url = left_url;
            book.Right_Url = right_url;
            book.Summary = bookview.Summary;
            book.AudioBook_Url = AudioBookUrl;
            book.Author = bookview.Author;

            price.Condition = conditiondata[0];
            price.ItemPrice = bookview.price;
            price.Book = book;
            price.Quantity = bookview.quantity;
            price.UpdatedBy = bookview.UpdatedBy;
            price.UpdatedOn = bookview.UpdatedOn;
            price.Active = bookview.Active;

            var books = _context.Book.Where(temp => temp.Name == book.Name && temp.Type == book.Type && temp.Publisher == book.Publisher && temp.Genre == book.Genre).ToList();
            if (books.Count == 0)
            {
                SaveBook(book);
                SavePrice(price);
            }

            else
            {
                price.Book = books[0];
                var prices = _context.Price.Where(p => p.Condition == price.Condition && p.Book.Name == book.Name).ToList();
                if (prices.Count == 0)
                {

                    SavePrice(price);
                }

                else
                {
                    var output = _context.Price.Where(p => p.Condition == price.Condition && p.Book.Name == book.Name).ToList();
                    output[0].Quantity = bookview.quantity;
                    output[0].ItemPrice = bookview.price;
                    _context.SaveChanges();
                }

            }

            return 1;
        }

         private IQueryable<Price> GetBaseOrderQuery()
        {
            var query = _context.Price
                           .Include(price => price.Condition)
                           .Include(price => price.Book);
            return query;
        }
       
        private  int GetTotalPageCount(List<BookDetails> FilteredBooks)
        {
            _logger.LogInformation("Calculating number of page for given query");

            var totalPages = 0;
            if ((FilteredBooks.Count() % _booksPerPage) == 0)
            {
                totalPages = (FilteredBooks.Count() / _booksPerPage);
            }
            else
            {
                totalPages = (FilteredBooks.Count() / _booksPerPage) + 1;
            }

            return totalPages;
        }

        public IEnumerable<BookDetails> GetDetails(long BookId)
        {
            var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == BookId select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
            return booker;
        }

        private Expression<Func<Price, bool>> GenerateDynamicLambdaFunction(string input, ParameterExpression parameterExpression, string searchString)
        {
            _logger.LogInformation("Generating Dynamic Lambda Function");

            Expression<Func<Price, bool>> lambda;

            var constant = Expression.Constant(searchString);

            Expression property2 = parameterExpression;
           
                foreach (var member in input.Split('.'))
                {
                    property2 = Expression.PropertyOrField(property2, member);
                }

                var method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });

                var expression = Expression.Call(property2, method, constant);

                lambda = Expression.Lambda<Func<Price, bool>>(expression, parameterExpression);

            return lambda;

        }
       

        private Expression<Func<Price,object>> GenerateDynamicLambdaSortedFunction(string input, ParameterExpression parameterExpression)
        {
            _logger.LogInformation("Generating Dynamic Lambda Function for sorting ");

            var splitter = input.Split(" ");
            Expression<Func<Price,object>> lambda;

            Expression property2 = parameterExpression;

              foreach (var member in splitter[1].Split('.'))
              {
                    property2 = Expression.PropertyOrField(property2, member);
              }

            lambda = Expression.Lambda<Func<Price,object>>(property2, parameterExpression);

                return lambda;                 
        }

        private Expression<Func<Price, int>> GenerateDynamicLambdaSortedVariantFunction(string input, ParameterExpression parameterExpression)
        {

            var splitter = input.Split(" ");
            Expression<Func<Price, int>> lambda;

            Expression property2 = parameterExpression;

            foreach (var member in splitter[1].Split('.'))
            {
                property2 = Expression.PropertyOrField(property2, member);
            }

            lambda = Expression.Lambda<Func<Price, int>>(property2, parameterExpression);

            return lambda;
        }

        public PagedSearchViewModel GetAllBooks(int pagenum, string style, string SortBy)
        {
            _logger.LogInformation("Preparing default search results ");

            var _booksPerPage = 15;
            var totalPages = 0;
            List<long> BookIdList = new List<long>();
            List<string> names = new List<string>();
            List<BookDetails> FilteredBooks = new List<BookDetails>();
            List<Price> books = new List<Price>();
            var query = GetBaseOrderQuery();
            Dictionary<string, double> Book_price = new Dictionary<string, double>();

            Dictionary<string, int> Book_qty = new Dictionary<string, int>();

            var parameterExpression = Expression.Parameter(Type.GetType("BOBS_Backend.Models.Book.Price"), "price");

            Expression<Func<Price, object>> lambda_sorting = null;

            Expression<Func<Price, int>> lambda_int_sort = null;

           
                books = query.ToList();

            foreach (var i in books)
            {
                if (!Book_price.ContainsKey(i.Book.Name))
                {

                    Book_price[i.Book.Name] = i.ItemPrice * i.Quantity;
                }

                else
                {
                    var data = Book_price.GetValueOrDefault(i.Book.Name);
                    Book_price[i.Book.Name] = data + i.ItemPrice * i.Quantity;
                }

                if (!Book_qty.ContainsKey(i.Book.Name))
                {

                    Book_qty[i.Book.Name] = i.Quantity;
                }

                else
                {
                    var dat = Book_qty.GetValueOrDefault(i.Book.Name);
                    Book_qty[i.Book.Name] = dat + i.Quantity;
                }

                if (!names.Contains(i.Book.Name))
                {
                    BookIdList.Add(i.Book.Book_Id);
                    names.Add(i.Book.Name);
                }
            }

            foreach (long i in BookIdList)
            {

                var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == i select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = Book_price[booke.Name] / Book_qty[booke.Name], Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = Book_qty[booke.Name], front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url, Author = booke.Author }).ToList();
                FilteredBooks.Add(book[0]);
            }


            if (!String.IsNullOrEmpty(SortBy))
            {
                if (SortBy.Contains("Quantity"))
                {
                    FilteredBooks = FilteredBooks.OrderBy(x => x.Quantity).ToList();

                }

                if (SortBy.Contains("ItemPrice"))
                {
                    FilteredBooks = FilteredBooks.OrderBy(x => x.Price).ToList();
                }

                if (SortBy.Contains("Name"))
                {

                    FilteredBooks = FilteredBooks.OrderBy(x => x.BookName).ToList();
                }

                if (SortBy.Contains("Author"))
                {

                    FilteredBooks = FilteredBooks.OrderBy(x => x.Author).ToList();
                }

            }

           
            totalPages = GetTotalPageCount(FilteredBooks);
            int[] pages = Enumerable.Range(1, totalPages).ToArray();

            PagedSearchViewModel viewModel = new PagedSearchViewModel();

            viewModel.searchby = "";
            viewModel.searchfilter = "";
            viewModel.Books = FilteredBooks.Skip((pagenum - 1) * _booksPerPage).Take(_booksPerPage);
            viewModel.Pages = pages;
            viewModel.HasPreviousPages = (pagenum > 1);
            viewModel.CurrentPage = pagenum;
            viewModel.HasNextPages = (pagenum < totalPages);
            if (string.IsNullOrEmpty(style))
            {
                viewModel.ViewStyle = "Tabular";
            }
            else
            {
                viewModel.ViewStyle = style;
            }

            return viewModel;
        }

        public PagedSearchViewModel SearchBeta(string searchby, string searchfilter , string style , string SortBy , int pagenum)
        {
            _logger.LogInformation("Preparing search pages for given search query ");

            var _booksPerPage = 15;
            var totalPages = 0;
            List<long> BookIdList = new List<long>();
            List<string> BookNames = new List<string>();
            List<BookDetails> FilteredBooks = new List<BookDetails>();
            List<Price> books = new List<Price>();

            Dictionary<string, double> Book_price = new Dictionary<string,double>();

            Dictionary<string, int> Book_qty = new Dictionary<string, int>();

            var parameterExpression = Expression.Parameter(Type.GetType("BOBS_Backend.Models.Book.Price"), "price");

            Expression<Func<Price, bool>> lambda;
            Expression<Func<Price,object>> lambda_sorting = null;
            Expression<Func<Price, int>> lambda_int_sort = null;           
                   
            var query = GetBaseOrderQuery();
            if (!String.IsNullOrEmpty(searchby))
            {
                lambda = GenerateDynamicLambdaFunction(searchby, parameterExpression, searchfilter);           
                books = query.Where(lambda).ToList();
            }

            else
            {             
                    books = query.Where(p => p.Book.Name.Contains(searchfilter) || p.Book.Publisher.Name.Contains(searchfilter) || p.Book.Genre.Name.Contains(searchfilter) || p.Condition.ConditionName.Contains(searchfilter) || p.Book.Type.TypeName.Contains(searchfilter)).ToList();
            }

            foreach (var i in books)
            {
                if(!Book_price.ContainsKey(i.Book.Name))
                {
                  
                    Book_price[i.Book.Name] = i.ItemPrice * i.Quantity;
                }

                else
                {
                  var data =  Book_price.GetValueOrDefault(i.Book.Name);
                    Book_price[i.Book.Name] = data + i.ItemPrice * i.Quantity;
                }

                if (!Book_qty.ContainsKey(i.Book.Name))
                {

                    Book_qty[i.Book.Name] = i.Quantity;
                }

                else
                {
                    var dat = Book_qty.GetValueOrDefault(i.Book.Name);
                    Book_qty[i.Book.Name] = dat + i.Quantity;
                }
                if (!BookNames.Contains(i.Book.Name))
                {
                    BookIdList.Add(i.Book.Book_Id);
                    BookNames.Add(i.Book.Name);                  
                }
            }

            var datao = Book_price.Count();
            var datum = BookIdList.Count();

            foreach (long i in BookIdList)
            {
                var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == i select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = Book_price[booke.Name]/ Book_qty[booke.Name], Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = Book_qty[booke.Name], front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url , Author = booke.Author}).ToList();
                FilteredBooks.Add(book[0]);
            }

            if (!String.IsNullOrEmpty(SortBy))
            {
                if (SortBy.Contains("Quantity"))
                {
                    FilteredBooks = FilteredBooks.OrderBy(x => x.Quantity).ToList();

                }

                if (SortBy.Contains("ItemPrice"))
                {
                    FilteredBooks = FilteredBooks.OrderBy( x => x.Price).ToList();
                }

                if (SortBy.Contains("Name"))
                {
                    
                    FilteredBooks = FilteredBooks.OrderBy(x => x.BookName).ToList();
                }

                if (SortBy.Contains("Author"))
                {

                    FilteredBooks = FilteredBooks.OrderBy(x => x.Author).ToList();
                }

            }

            totalPages = GetTotalPageCount(FilteredBooks);
            int[] pages = Enumerable.Range(1, totalPages).ToArray();

            PagedSearchViewModel viewModel = new PagedSearchViewModel();

            viewModel.searchby = searchby;
            viewModel.searchfilter = searchfilter;
            viewModel.Books = FilteredBooks.Skip((pagenum - 1) * _booksPerPage).Take(_booksPerPage);
            viewModel.Pages = pages;
            viewModel.HasPreviousPages = (pagenum > 1);
            viewModel.CurrentPage = pagenum;
            viewModel.HasNextPages = (pagenum < totalPages);
            if (string.IsNullOrEmpty(style))
            {
                viewModel.ViewStyle = "Tabular";
            }
            else
            {
                viewModel.ViewStyle = style;
            }

            return viewModel;
        }

        public List<string> GetFormatsOfTheSelectedBook(string bookname)
        {
            _logger.LogInformation("Fetching all possible formats of the given book");

            List<string> types = new List<string>();
            var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name ==  bookname select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();

            foreach (var i in book)
            {
                if (!types.Contains(i.BookType.TypeName))
                {
                    types.Add(i.BookType.TypeName);

                }
            }

            return types;
        }

        public List<string> GetConditionsOfTheSelectedBook(string bookname)
        {
            _logger.LogInformation("Fetching all possible conditions of the given book");

            List<string> conditions = new List<string>();
            var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name == bookname select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();

            foreach (var i in book)
            {
                if (!conditions.Contains(i.BookCondition.ConditionName))
                {
                    conditions.Add(i.BookCondition.ConditionName);

                }
            }

            return conditions;
        }

        public List<BookDetails> GetRelevantBooks(string Bookname , string type , string condition_chosen)
        {
            _logger.LogInformation("Fetching all possible books based on type and condition chosen");


            if (!string.IsNullOrEmpty(condition_chosen) && !string.IsNullOrEmpty(type))
            {
                var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name == Bookname && booke.Type.TypeName == type && price.Condition.ConditionName == condition_chosen select new BookDetails { Author = booke.Author ,BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();
                return book;
            }

            if (!string.IsNullOrEmpty(condition_chosen))
            {
                var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name == Bookname && price.Condition.ConditionName == condition_chosen select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url ,Author = booke.Author }).ToList();
                return book;
            }

            if (!string.IsNullOrEmpty(type))
            {
                var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name == Bookname && price.Condition.ConditionName == condition_chosen select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url , Author = booke.Author}).ToList();
                return book;
            }

            var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name == Bookname select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url , Author = booke.Author}).ToList();
            return booker;
    
        }

        /*
        *  function to fetch all calculate the dashboard statistics 
        */

        private List<Dictionary<string, int>> topfivestatsforOrders()
        {
            _logger.LogInformation("calculating Top 5 stats for orders");

            Dictionary<string, int> genre_stats = new Dictionary<string, int>();
            Dictionary<string, int> publisher_stats = new Dictionary<string, int>();
            Dictionary<string, int> condition_stats = new Dictionary<string, int>();
            Dictionary<string, int> bookname_stats = new Dictionary<string, int>();
            Dictionary<string, int> type_stats = new Dictionary<string, int>();
            List<Dictionary<string, int>> topfiveStats = new List<Dictionary<string, int>>();


            var list = _context.OrderDetail
                        .Include(o => o.Book)
                            .ThenInclude(b => b.Genre)
                        .Include(o => o.Book)
                            .ThenInclude(b => b.Publisher)
                        .Include(o => o.Book)
                            .ThenInclude(b => b.Type)
                        .Include(o => o.Price)
                            .ThenInclude(p => p.Condition)
                        .ToList();

            foreach (var detail in list)
            {
                if (!genre_stats.ContainsKey(detail.Book.Genre.Name))
                {
                    genre_stats.Add(detail.Book.Genre.Name, 1);
                }

                else
                {
                    var count = genre_stats.GetValueOrDefault(detail.Book.Genre.Name);
                    genre_stats[detail.Book.Genre.Name] = count + 1;
                }

                if (!publisher_stats.ContainsKey(detail.Book.Publisher.Name))
                {
                    publisher_stats.Add(detail.Book.Publisher.Name, 1);
                }

                else
                {
                    var count = publisher_stats.GetValueOrDefault(detail.Book.Publisher.Name);
                    publisher_stats[detail.Book.Publisher.Name] = count + 1;
                }

                if (!bookname_stats.ContainsKey(detail.Book.Name))
                {
                    bookname_stats.Add(detail.Book.Name, 1);
                }

                else
                {
                    var count = bookname_stats.GetValueOrDefault(detail.Book.Name);
                    bookname_stats[detail.Book.Name] = count + 1;
                }

                if (!type_stats.ContainsKey(detail.Book.Type.TypeName))
                {
                    type_stats.Add(detail.Book.Type.TypeName, 1);
                }

                else
                {
                    var count = type_stats.GetValueOrDefault(detail.Book.Type.TypeName);
                    type_stats[detail.Book.Type.TypeName] = count + 1;
                }
            }

            try
            {
                genre_stats = (from entry in genre_stats orderby entry.Value descending select entry)
                        .Take(5)
                        .ToDictionary(pair => pair.Key, pair => pair.Value);

                type_stats = (from entry in type_stats orderby entry.Value descending select entry)
                        .Take(5)
                        .ToDictionary(pair => pair.Key, pair => pair.Value);

                publisher_stats = (from entry in publisher_stats orderby entry.Value descending select entry)
                        .Take(5)
                        .ToDictionary(pair => pair.Key, pair => pair.Value);

                bookname_stats = (from entry in bookname_stats orderby entry.Value descending select entry)
                        .Take(5)
                        .ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            catch
            {
                Console.WriteLine("Error In Fetching Dashboard Top 5 Stats");
            }

            topfiveStats.Add(genre_stats);
            topfiveStats.Add(type_stats);
            topfiveStats.Add(publisher_stats);
            topfiveStats.Add(bookname_stats);

            return topfiveStats;
        }


        private Dictionary<string, int>  InventoryStats()
        {
            _logger.LogInformation("calculating Inventory stats");

            int total_merchandise_value = 0;
            int total_books_inventory = 0;
            int total_titles = 0;

            Dictionary<string, int> count_stats = new Dictionary<string, int>();

            try
            {
                var books = GetBaseOrderQuery().ToList();
                foreach (var i in books)
                {
                    total_books_inventory = total_books_inventory + i.Quantity;
                    total_merchandise_value = total_merchandise_value + (int)i.ItemPrice * i.Quantity;
                }

                total_titles = _context.Book.ToList().Count();

                count_stats["total_titles"] = total_titles;
                count_stats["total_books"] = total_books_inventory;
                count_stats["total_merchandise_value"] = total_merchandise_value;
                count_stats["total_genres"] = GetGenres().Count();
                count_stats["total_publishers"] = GetAllPublishers().Count();
                count_stats["total_types"] = GetTypes().Count();
                count_stats["total_conditions"] = GetConditions().Count();

            }

            catch
            {
                Console.WriteLine("Error in Fetching Inventory Stats");
            }
            return count_stats;
        }

        private Dictionary<string, int> OrderStats()
        {
            _logger.LogInformation("calculating Order stats");

            double total_sales_value = 0;
            int total_quantity_sold = 0;

            Dictionary<string, int> count_stats = new Dictionary<string, int>();
            
                var delivered = _context.Order.Where(x => x.OrderStatus.Status == "Delivered").ToList().Count();
                var justplaced = _context.Order.Where(x => x.OrderStatus.Status == "Just Placed").ToList().Count();
                var enroute = _context.Order.Where(x => x.OrderStatus.Status == "En Route").ToList().Count();
                var pending = _context.Order.Where(x => x.OrderStatus.Status == "Pending").ToList().Count();
                var ordersplaced = delivered + justplaced + enroute + pending;

                var orderdetailsList = _context.OrderDetail.ToList();
                foreach (var i in orderdetailsList)
                {
                    total_quantity_sold = total_quantity_sold + i.quantity;
                    total_sales_value = total_sales_value + i.price * i.quantity;
                }

           
            count_stats["Orders_placed"] = ordersplaced;
            count_stats["delivered"] = delivered;
            count_stats["enroute"] = enroute;
            count_stats["pending"] = pending;
            count_stats["justplaced"] = justplaced;
            count_stats["total_books_sold"] = total_quantity_sold;
            count_stats["total_sales"] = (int)total_sales_value;


            

           
                Console.WriteLine("Error in Fetching Order Stats");

            

            return count_stats;

        }
        public List<Dictionary<string,int>> DashBoard()
        {
            _logger.LogInformation("Packing together dashboard results ");

            var topfive = topfivestatsforOrders();
            var Inventory = InventoryStats();
            var Orders = OrderStats();

            topfive.Add(Inventory);
            topfive.Add(Orders);

            return topfive;
        }

        /*
        *  function to update details of an existing book
        */
        public BookDetails UpdateDetails(int Id, string Condition)
        {
            _logger.LogInformation("Fetching data to pre-populate edit page ");

            var list = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == Id && price.Condition.ConditionName == Condition select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();

            return list[0];
        }

        /*
        *  function to update details of an existing book
        */
        public void PushDetails(BookDetails details)
        {
            _logger.LogInformation("Pushing edited details to database");

            var output = _context.Price.Where(p => p.Condition.ConditionName == details.BookCondition.ConditionName && p.Book.Book_Id == details.BookId).ToList();
            if (details.FrontPhoto != null)
            {
                details.front_url = _RekognitionNPollyRepository.UploadtoS3(details.FrontPhoto, details.BookId, details.BookCondition.ConditionName).Result;
            }

            if (details.BackPhoto != null)
            {
                details.back_url = _RekognitionNPollyRepository.UploadtoS3(details.BackPhoto, details.BookId, details.BookCondition.ConditionName).Result;
            }


            if (details.LeftSidePhoto != null)
            {
                details.left_url = _RekognitionNPollyRepository.UploadtoS3(details.LeftSidePhoto, details.BookId, details.BookCondition.ConditionName).Result;
            }

            if (details.RightSidePhoto != null)
            {
                details.right_url = _RekognitionNPollyRepository.UploadtoS3(details.RightSidePhoto, details.BookId, details.BookCondition.ConditionName).Result;
            }

            var book = _context.Book.Where(p => p.Book_Id == details.BookId).ToList();
            book[0].Front_Url = details.front_url;
            book[0].Back_Url = details.back_url;
            book[0].Left_Url = details.left_url;
            book[0].Right_Url = details.right_url;
            output[0].Quantity = details.Quantity;
            output[0].ItemPrice = details.Price;
            output[0].UpdatedBy = details.UpdatedBy;
            output[0].UpdatedOn = details.UpdatedOn;
            output[0].Active = details.Active;
            _context.SaveChanges();
        }

        /*
        *  function to display autosuggestions that enhances search functionality
        */
        public List<string> autosuggest(string input)
        {
            _logger.LogInformation("Preparing autosuggestions");

            List<string> names = new List<string>();

           var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name.ToLower().Contains(input.ToLower()) select new BookDetails { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, front_url = booke.Front_Url, back_url = booke.Back_Url, left_url = booke.Left_Url, right_url = booke.Right_Url }).ToList();


            foreach (var i in booker)
            {
                if (!names.Contains(i.BookName))
                {
                    names.Add(i.BookName);

                }
            }
         
            foreach (var i in GetGenres())
            {
                if (i.Name.ToLower().Contains(input.ToLower()))
                {
                    names.Add(i.Name);
                }
            }

            foreach (var i in GetAllPublishers())
            {
                if (i.Name.ToLower().Contains(input.ToLower()))
                {
                    names.Add(i.Name);
                }
            }

            foreach (var i in GetTypes())
            {
                if (i.TypeName.ToLower().Contains(input.ToLower()))
                {
                    names.Add(i.TypeName);
                }
            }
            foreach (var i in GetConditions())
            {
                if (i.ConditionName.ToLower().Contains(input.ToLower()))
                {
                    names.Add(i.ConditionName);
                }
            }


            return names;
        }


    }


}



