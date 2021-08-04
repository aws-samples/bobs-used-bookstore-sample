using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Type = System.Type;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using Amazon.Polly;
using BookstoreBackend.Database;
using BobsBookstore.Models.Books;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.DataAccess.Repository.Interface.InventoryInterface;
using BobsBookstore.DataAccess.Repository.Interface.Implementations;
using BobsBookstore.DataAccess.Repository.Interface.SearchImplementations;
using BobsBookstore.DataAccess.Dtos;

namespace BobsBookstore.DataAccess.Repository.Implementation.InventoryImplementation
{
    /*
        * Inventory Repository contains all functions associated with managing and viewing Inventory
        */

    public class Inventory : IInventory
    {

        public ApplicationDbContext _context;
        private readonly int _booksPerPage = 15;
        private readonly string[] PriceIncludes = { "Book", "Condition" };
        private readonly IRekognitionNPollyRepository _RekognitionNPollyRepository;
        private readonly ISearchRepository _searchRepo;
        private readonly ILogger<Inventory> _logger;


        public Inventory(ApplicationDbContext context, ISearchRepository searchRepository, IRekognitionNPollyRepository RekognitionNPollyRepository, ILogger<Inventory> logger)
        {
            _context = context;
            _searchRepo = searchRepository;
            _RekognitionNPollyRepository = RekognitionNPollyRepository;
            _logger = logger;
        }

        public BookDetailsDto GetBookByID(long bookId)
        {

            var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == bookId select new BookDetailsDto {Summary = booke.Summary, ISBN = booke.ISBN ,Author = booke.Author, BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, FrontUrl = booke.FrontUrl, BackUrl = booke.BackUrl, LeftUrl = booke.LeftUrl, RightUrl = booke.RightUrl }).ToList();
            if (book.Count > 0)
            {
                return book[0];
            }

            return null;
      
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

        public bool AddPublishers(BobsBookstore.Models.Books.Publisher publishers)
        {
            _logger.LogInformation("Posting details to the Publisher table");
            var publishName = _context.Publisher.Where(publisher => publisher.Name == publishers.Name).ToList();
            if (publishName.Count == 0)
            {
                _context.Publisher.Add(publishers);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        /*
         *  function to add  details to the Genres table
         */
        public bool AddGenres(BobsBookstore.Models.Books.Genre genres)
        {
            _logger.LogInformation("Posting details to the Genres table");
            var genreName = _context.Genre.Where(genre => genre.Name == genres.Name).ToList();
            if (genreName.Count == 0)
            {
                _context.Genre.Add(genres);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        /*
         *  function to add  details to the Types table
         */
        public bool AddBookTypes(BobsBookstore.Models.Books.Type booktype)
        {
            _logger.LogInformation("Posting details to the Types table");
            var typeName = _context.Type.Where(type => type.TypeName == booktype.TypeName).ToList();
            var typeId = _context.Type.Where(type => type.Type_Id == booktype.Type_Id).ToList();

            if (typeName.Count == 0)
            {
                _context.Type.Add(booktype);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        /*
         *  function to add  details to the Conditions table
         */
        public bool AddBookConditions(Condition bookCondition)
        {
            _logger.LogInformation("Posting details to the Conditions table");
           
            var conditionName = _context.Condition.Where(condition => condition.ConditionName == bookCondition.ConditionName).ToList();

            if (conditionName.Count == 0)
            {
                _context.Condition.Add(bookCondition);
                _context.SaveChanges();
                return true;
            }
            return false;

        }

        public void EditPublisher(string oldPublisherName , string newPublisherName)
        {
            _logger.LogInformation("Posting details to the Conditions table");

            try
            {
                var publishName = _context.Publisher.Where(x => x.Name == oldPublisherName).ToList();
                using (var transaction = _context.Database.BeginTransaction())
                {
                    publishName[0].Name = newPublisherName;
                    _context.SaveChanges();
                    transaction.Commit();
                }

            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "DBConcurrency Error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }

        }

        public void EditGenre(string oldGenre, string newGenre)
        {
            _logger.LogInformation("Posting details to the Conditions table");
            try
            {
                var GenreName = _context.Genre.Where(x => x.Name == oldGenre).ToList();
                using (var transaction = _context.Database.BeginTransaction())
                {
                    GenreName[0].Name = newGenre;
                    _context.SaveChanges();
                }
            }

            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "DBConcurrency Error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }

        }

        public void EditCondition(string oldCondition, string newCondition)
        {
            _logger.LogInformation("Posting details to the Conditions table");
            try
            {
                var conditionName = _context.Condition.Where(x => x.ConditionName == oldCondition).ToList();
                using (var transaction = _context.Database.BeginTransaction())
                {
                    conditionName[0].ConditionName = newCondition;
                    _context.SaveChanges();
                }
            }

            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "DBConcurrency Error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }

        }

        public void EditType(string oldType, string newType)
        {
            _logger.LogInformation("Posting details to the Conditions table");
            try
            {
                var typeName = _context.Type.Where(x => x.TypeName == oldType).ToList();
                using (var transaction = _context.Database.BeginTransaction())
                {
                    typeName[0].TypeName = newType;
                    _context.SaveChanges();
                }
            }

            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "DBConcurrency Error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }


        }

        /*
         *  function to fetch all publisher details for dynamic display in the drop down list 
         */
        public List<BobsBookstore.Models.Books.Publisher> GetAllPublishers()
        {
            var publishers = _context.Publisher.ToList();
            return publishers;
        }


        /*
        *  function to fetch all Genres details for dynamic display in the drop down list 
        */
        public List<BobsBookstore.Models.Books.Genre> GetGenres()
        {

            var genres = _context.Genre.ToList();
            return genres;
        }

        /*
        *  function to fetch all Types details for dynamic display in the drop down list 
        */
        public List<BobsBookstore.Models.Books.Type> GetTypes()
        {

            var typelist = _context.Type.ToList();
            return typelist;
        }

        /*
        *  function to fetch all Conditions for dynamic display in the drop down list 
        */
        public List<BobsBookstore.Models.Books.Condition> GetConditions()
        {

            var conditions = _context.Condition.ToList();
            return conditions;
        }
        
         /*
         *  function to fetch Book details given its BookId and Condition t 
         */
        public BookDetailsDto GetBookDetails(long bookId, long priceId)
        {

            var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == bookId && price.Price_Id == priceId select new BookDetailsDto { BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, FrontUrl = booke.FrontUrl, BackUrl = booke.BackUrl, LeftUrl = booke.LeftUrl, RightUrl = booke.RightUrl }).ToList();
            return booker[0];
        }

        /*
        *  function to fetch all process form inputs and add details to the database 
        */
        public bool AddToTables(BooksDto booksDto)
        {
            _logger.LogInformation("Processing and Posting data to tables and cloud ");

            try
            {
                string frontUrl = "", backUrl = "", leftUrl = "", rightUrl = "", audioBookUrl = "";
                if (booksDto.FrontPhoto != null)
                {
                    frontUrl = _RekognitionNPollyRepository.UploadtoS3(booksDto.FrontPhoto, booksDto.BookId, booksDto.BookCondition).Result;
                }

                if (booksDto.BackPhoto != null)
                {
                    backUrl = _RekognitionNPollyRepository.UploadtoS3(booksDto.BackPhoto, booksDto.BookId, booksDto.BookCondition).Result;
                }

                if (booksDto.LeftSidePhoto != null)
                {
                    leftUrl = _RekognitionNPollyRepository.UploadtoS3(booksDto.LeftSidePhoto, booksDto.BookId, booksDto.BookCondition).Result;
                }

                if (booksDto.RightSidePhoto != null)
                {
                    rightUrl = _RekognitionNPollyRepository.UploadtoS3(booksDto.RightSidePhoto, booksDto.BookId, booksDto.BookCondition).Result;
                }

                if (_RekognitionNPollyRepository.IsContentViolation(frontUrl) == true || _RekognitionNPollyRepository.IsContentViolation(backUrl) == true || _RekognitionNPollyRepository.IsContentViolation(leftUrl) == true || _RekognitionNPollyRepository.IsContentViolation(rightUrl) == true)
                {
                    return false;
                }

                if (booksDto.Summary != null)
                {
                    audioBookUrl = _RekognitionNPollyRepository.GenerateAudioSummary(booksDto.BookName, booksDto.Summary, ConstantsData.TextToSpeechLanguageCode, VoiceId.Emma);
                }

                Book book = new Book();
                Price price = new Price();

                var publisherdata = _context.Publisher.Where(publisher => publisher.Name == booksDto.PublisherName).ToList();
                var genredata = _context.Genre.Where(genre => genre.Name == booksDto.Genre).ToList();
                var typedata = _context.Type.Where(type => type.TypeName == booksDto.BookType).ToList();
                var conditiondata = _context.Condition.Where(condition => condition.ConditionName == booksDto.BookCondition).ToList();

                book.Name = booksDto.BookName;
                book.Type = typedata[0];
                book.Genre = genredata[0];
                book.ISBN = booksDto.ISBN;
                book.Publisher = publisherdata[0];
                book.FrontUrl = frontUrl;
                book.BackUrl = backUrl;
                book.LeftUrl = leftUrl;
                book.RightUrl = rightUrl;
                book.Summary = booksDto.Summary;
                book.AudioBookUrl = audioBookUrl;
                book.Author = booksDto.Author;

                price.Condition = conditiondata[0];
                price.ItemPrice = booksDto.Price;
                price.Book = book;
                price.Quantity = booksDto.Quantity;
                price.UpdatedBy = booksDto.UpdatedBy;
                price.UpdatedOn = booksDto.UpdatedOn;
                price.Active = booksDto.Active;

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
                        output[0].Quantity = booksDto.Quantity;
                        output[0].ItemPrice = booksDto.Price;
                        _context.SaveChanges();
                    }

                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error in AddBooksFunc");
            }
            return true;
        }

         private IQueryable<Price> GetBaseOrderQuery()
        {
            var query = _context.Price
                           .Include(price => price.Condition)
                           .Include(price => price.Book);
            return query;
        }
       

        public IEnumerable<BookDetailsDto> GetDetails(long bookId)
        {
            var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == bookId select new BookDetailsDto { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, FrontUrl = booke.FrontUrl, BackUrl = booke.BackUrl, LeftUrl = booke.LeftUrl, RightUrl = booke.RightUrl }).ToList();
            return booker;
        }

        private List<FullBookDto> RetrieveUniqueBookList(IQueryable<Price> query)
        {
            List<Price> books = null;

            List<string> BookIds = new List<string>();
            List<FullBookDto> finalBooksList = new List<FullBookDto>();

            books = query.ToList();
            foreach (var book in books)
            {
                if (!BookIds.Contains(book.Book.Name))
                {
                    BookIds.Add(book.Book.Name);
                    finalBooksList.Add(new FullBookDto { LowestPrice = (int)book.ItemPrice, TotalQuantity = book.Quantity, Price = book });
                }
                else
                {
                    var fullBook = finalBooksList.Where(find => find.Price.Book.Name == book.Book.Name).FirstOrDefault();
                    fullBook.TotalQuantity += book.Quantity;
                    fullBook.LowestPrice = (fullBook.LowestPrice > (int)book.ItemPrice) ? (int)book.ItemPrice : fullBook.LowestPrice;
                }

            }

            return finalBooksList;
        }

        private List<FullBookDto> RetrieveSortedBookList(List<FullBookDto> finalBooksList, string sortBy, string ascDesc)
        {
            var parameterExpression2 = Expression.Parameter(Type.GetType("BobsBookstore.DataAccess.Dtos.FullBookDto"), "fullbook");

            Expression property = null;

            if (String.IsNullOrEmpty(sortBy))
            {
                return finalBooksList;
            }
            else if (!sortBy.Contains("."))
            {
                property = Expression.Property(parameterExpression2, sortBy);

                var _sortBy = Expression.Lambda<Func<FullBookDto, int>>(property, new[] { parameterExpression2 });

                if (string.IsNullOrEmpty(ascDesc)) ascDesc = "asc";

                var bookList = (ascDesc.Contains("asc")) ? finalBooksList.AsQueryable().OrderBy(_sortBy) : finalBooksList.AsQueryable().OrderByDescending(_sortBy);

                return bookList.ToList();
            }
            else
            {
                property = parameterExpression2;
                foreach (var member in sortBy.Split('.'))
                {
                    property = Expression.PropertyOrField(property, member);
                }

                var _sortBy = Expression.Lambda<Func<FullBookDto, string>>(property, new[] { parameterExpression2 });

                if (string.IsNullOrEmpty(ascDesc)) ascDesc = "asc";

                var bookList = (ascDesc.Contains("asc")) ? finalBooksList.AsQueryable().OrderBy(_sortBy) : finalBooksList.AsQueryable().OrderByDescending(_sortBy);

                return bookList.ToList();

            }
        }

        private SearchBookDto RetrieveDto(string ascDesc, string style, string filterValue, string searchString, int pageNum, int totalPages, int[] pages, List<FullBookDto> books)
        {
            SearchBookDto viewModel = new SearchBookDto();

            viewModel.Searchby = filterValue;
            viewModel.Searchfilter = searchString;
            viewModel.Books = books;
            viewModel.Pages = pages;
            viewModel.HasPreviousPages = (pageNum > 1);
            viewModel.CurrentPage = pageNum;
            viewModel.HasNextPages = (pageNum < totalPages);
            viewModel.Ascdesc = ascDesc;
            viewModel.ViewStyle = style;

            return viewModel;
        }

        private SearchBookDto RetrieveFilterDto(List<FullBookDto> filterQuery, string ascDesc, string style, int totalPages, int pageNum, string filterValue, string searchString)
        {
            SearchBookDto viewModel = new SearchBookDto();

            var books = filterQuery
                            .Skip((pageNum - 1) * _booksPerPage)
                            .Take(_booksPerPage)
                            .ToList();

            int[] pages = _searchRepo.GetModifiedPagesArr(pageNum, totalPages);

            var searchBookDto = RetrieveDto(ascDesc, style, filterValue, searchString, pageNum, totalPages, pages, books);

            return searchBookDto;

        }

        public SearchBookDto GetAllBooks(int pageNum, string style, string sortBy, string ascDesc)
        {

            var query = (IQueryable<Price>)_context.Price;

            query = query.Include(PriceIncludes);

            var booksList = RetrieveUniqueBookList(query);

            var finalBooksList = RetrieveSortedBookList(booksList, sortBy, ascDesc);

            int totalPages = _searchRepo.GetTotalPages(finalBooksList.Count(), _booksPerPage);


            SearchBookDto viewModel = new SearchBookDto();

            viewModel = RetrieveFilterDto(finalBooksList, ascDesc, style, totalPages, pageNum, "", "");

            return viewModel;

        }

        public SearchBookDto SearchBooks(string searchBy, string searchFilter, string style, string sortBy, int pageNum, string ascDesc)
        {

            searchBy = " " + searchBy;

            var parameterExpression = Expression.Parameter(Type.GetType("BobsBookstore.Models.Books.Price"), "order");


            var expression = _searchRepo.ReturnExpression(parameterExpression, searchBy, searchFilter);

            searchBy = searchBy.Trim();
            Expression<Func<Price, bool>> lambda = Expression.Lambda<Func<Price, bool>>(expression, parameterExpression);

            if (lambda == null)
            {
                int[] pages = Enumerable.Range(1, 1).ToArray();

                return RetrieveDto(ascDesc, style, "", "", 1, 1, pages, null);
            }

            var query = (IQueryable<Price>)_context.Price.Include(PriceIncludes); ;

            var filterQuery = query.Where(lambda);

            var booksList = RetrieveUniqueBookList(filterQuery);

            var finalBooksList = RetrieveSortedBookList(booksList, sortBy, ascDesc);

            int totalPages = _searchRepo.GetTotalPages(finalBooksList.Count(), _booksPerPage);

            var searchBookDto = RetrieveFilterDto(finalBooksList, ascDesc, style, totalPages, pageNum, searchBy, searchFilter);

            return searchBookDto;
        }
        public List<string> GetFormatsOfTheSelectedBook(string bookName)
        {
            _logger.LogInformation("Fetching all possible formats of the given book");

            List<string> types = new List<string>();
            var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name ==  bookName select new BookDetailsDto { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, FrontUrl = booke.FrontUrl, BackUrl = booke.BackUrl, LeftUrl = booke.LeftUrl, RightUrl = booke.RightUrl }).ToList();

            foreach (var i in book)
            {
                if (!types.Contains(i.BookType.TypeName))
                {
                    types.Add(i.BookType.TypeName);

                }
            }

            return types;
        }

        public List<string> GetConditionsOfTheSelectedBook(string bookName)
        {
            _logger.LogInformation("Fetching all possible conditions of the given book");

            List<string> conditions = new List<string>();
            var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name == bookName select new BookDetailsDto { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, FrontUrl = booke.FrontUrl, BackUrl = booke.BackUrl, LeftUrl = booke.LeftUrl, RightUrl = booke.RightUrl }).ToList();

            foreach (var i in book)
            {
                if (!conditions.Contains(i.BookCondition.ConditionName))
                {
                    conditions.Add(i.BookCondition.ConditionName);

                }
            }

            return conditions;
        }

        public List<BookDetailsDto> GetRelevantBooks(string bookName , string type , string conditionChosen)
        {
            _logger.LogInformation("Fetching all possible books based on type and condition chosen");


            if (!string.IsNullOrEmpty(conditionChosen) && !string.IsNullOrEmpty(type))
            {
                var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name == bookName && booke.Type.TypeName == type && price.Condition.ConditionName == conditionChosen select new BookDetailsDto { Author = booke.Author ,BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, FrontUrl = booke.FrontUrl, BackUrl = booke.BackUrl, LeftUrl = booke.LeftUrl, RightUrl = booke.RightUrl }).ToList();
                return book;
            }

            if (!string.IsNullOrEmpty(conditionChosen))
            {
                var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name == bookName && price.Condition.ConditionName == conditionChosen select new BookDetailsDto { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, FrontUrl = booke.FrontUrl, BackUrl = booke.BackUrl, LeftUrl = booke.LeftUrl, RightUrl = booke.RightUrl ,Author = booke.Author }).ToList();
                return book;
            }

            if (!string.IsNullOrEmpty(type))
            {
                var book = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name == bookName && price.Condition.ConditionName == conditionChosen select new BookDetailsDto { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, FrontUrl = booke.FrontUrl, BackUrl = booke.BackUrl, LeftUrl = booke.LeftUrl, RightUrl = booke.RightUrl , Author = booke.Author}).ToList();
                return book;
            }

            var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name == bookName select new BookDetailsDto { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, FrontUrl = booke.FrontUrl, BackUrl = booke.BackUrl, LeftUrl = booke.LeftUrl, RightUrl = booke.RightUrl , Author = booke.Author}).ToList();
            return booker;
    
        }

        /*
        *  function to fetch all calculate the dashboard statistics 
        */

        private List<Dictionary<string, int>> topStatsForOrders(int numberOfDetails)
        {
            _logger.LogInformation("calculating Top stats for orders");

            Dictionary<string, int> genre_stats = new Dictionary<string, int>();
            Dictionary<string, int> publisher_stats = new Dictionary<string, int>();
            Dictionary<string, int> condition_stats = new Dictionary<string, int>();
            Dictionary<string, int> bookname_stats = new Dictionary<string, int>();
            Dictionary<string, int> type_stats = new Dictionary<string, int>();
            List<Dictionary<string, int>> topStats = new List<Dictionary<string, int>>();


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
                        .Take(numberOfDetails)
                        .ToDictionary(pair => pair.Key, pair => pair.Value);

                type_stats = (from entry in type_stats orderby entry.Value descending select entry)
                        .Take(numberOfDetails)
                        .ToDictionary(pair => pair.Key, pair => pair.Value);

                publisher_stats = (from entry in publisher_stats orderby entry.Value descending select entry)
                        .Take(numberOfDetails)
                        .ToDictionary(pair => pair.Key, pair => pair.Value);

                bookname_stats = (from entry in bookname_stats orderby entry.Value descending select entry)
                        .Take(numberOfDetails)
                        .ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            catch
            {
                Console.WriteLine("Error In Fetching Dashboard Top Stats");
            }

            topStats.Add(genre_stats);
            topStats.Add(type_stats);
            topStats.Add(publisher_stats);
            topStats.Add(bookname_stats);

            return topStats;
        }


        private Dictionary<string, int>  InventoryStats()
        {
            _logger.LogInformation("calculating Inventory stats");

            int totalMerchandiseValue = 0;
            int totalBooksInventory = 0;
            int total_titles = 0;

            Dictionary<string, int> count_stats = new Dictionary<string, int>();

            try
            {
                var books = GetBaseOrderQuery().ToList();
                foreach (var i in books)
                {
                    totalBooksInventory = totalBooksInventory + i.Quantity;
                    totalMerchandiseValue = totalMerchandiseValue + (int)i.ItemPrice * i.Quantity;
                }

                total_titles = _context.Book.ToList().Count();

                count_stats["total_titles"] = total_titles;
                count_stats["total_books"] = totalBooksInventory;
                count_stats["total_merchandise_value"] = totalMerchandiseValue;
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

            decimal total_sales_value = 0;
            int total_quantity_sold = 0;

            Dictionary<string, int> count_stats = new Dictionary<string, int>();
            
                var delivered = _context.Order.Where(x => x.OrderStatus.Status == ConstantsData.OrderStatusDelivered).ToList().Count();
                var justplaced = _context.Order.Where(x => x.OrderStatus.Status == ConstantsData.OrderStatusJustPlaced).ToList().Count();
                var enroute = _context.Order.Where(x => x.OrderStatus.Status == ConstantsData.OrderStatusEnRoute).ToList().Count();
                var pending = _context.Order.Where(x => x.OrderStatus.Status == ConstantsData.OrderStatusPending).ToList().Count();
                var ordersplaced = delivered + justplaced + enroute + pending;

                var orderdetailsList = _context.OrderDetail.ToList();
                foreach (var i in orderdetailsList)
                {
                    total_quantity_sold = total_quantity_sold + i.Quantity;
                    total_sales_value = total_sales_value + i.OrderDetailPrice * i.Quantity;
                }

           
            count_stats["Orders_placed"] = ordersplaced;
            count_stats[ConstantsData.OrderStatusDelivered] = delivered;
            count_stats[ConstantsData.OrderStatusEnRoute] = enroute;
            count_stats[ConstantsData.OrderStatusPending] = pending;
            count_stats[ConstantsData.OrderStatusJustPlaced] = justplaced;
            count_stats["total_books_sold"] = total_quantity_sold;
            count_stats["total_sales"] = (int)total_sales_value;


            

           
                Console.WriteLine("Error in Fetching Order Stats");

            

            return count_stats;

        }
        public List<Dictionary<string,int>> DashBoard(int numberOfDetails)
        {
            _logger.LogInformation("Packing together dashboard results ");
            
            var topfive = topStatsForOrders(numberOfDetails);
            var Inventory = InventoryStats();
            var Orders = OrderStats();

            topfive.Add(Inventory);
            topfive.Add(Orders);

            return topfive;
        }

        /*
        *  function to update details of an existing book
        */
        public BookDetailsDto UpdateDetails(int id, string condition)
        {
            _logger.LogInformation("Fetching data to pre-populate edit page ");

            var list = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Book_Id == id && price.Condition.ConditionName == condition select new BookDetailsDto {Author = booke.Author, Active = price.Active, BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, FrontUrl = booke.FrontUrl, BackUrl = booke.BackUrl, LeftUrl = booke.LeftUrl, RightUrl = booke.RightUrl }).ToList();

            return list[0];
        }

        /*
        *  function to update details of an existing book
        */
        public void PushDetails(BookDetailsDto bookdetailsDto)
        {
            _logger.LogInformation("Pushing edited details to database");

            try
            {
                var output = _context.Price.Where(p => p.Condition.ConditionName == bookdetailsDto.BookCondition.ConditionName && p.Book.Book_Id == bookdetailsDto.BookId).ToList();
                if (bookdetailsDto.FrontPhoto != null)
                {
                    bookdetailsDto.FrontUrl = _RekognitionNPollyRepository.UploadtoS3(bookdetailsDto.FrontPhoto, bookdetailsDto.BookId, bookdetailsDto.BookCondition.ConditionName).Result;
                }

                if (bookdetailsDto.BackPhoto != null)
                {
                    bookdetailsDto.BackUrl = _RekognitionNPollyRepository.UploadtoS3(bookdetailsDto.BackPhoto, bookdetailsDto.BookId, bookdetailsDto.BookCondition.ConditionName).Result;
                }


                if (bookdetailsDto.LeftSidePhoto != null)
                {
                    bookdetailsDto.LeftUrl = _RekognitionNPollyRepository.UploadtoS3(bookdetailsDto.LeftSidePhoto, bookdetailsDto.BookId, bookdetailsDto.BookCondition.ConditionName).Result;
                }

                if (bookdetailsDto.RightSidePhoto != null)
                {
                    bookdetailsDto.RightUrl = _RekognitionNPollyRepository.UploadtoS3(bookdetailsDto.RightSidePhoto, bookdetailsDto.BookId, bookdetailsDto.BookCondition.ConditionName).Result;
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    var book = _context.Book.Where(p => p.Book_Id == bookdetailsDto.BookId).ToList();
                    book[0].FrontUrl = bookdetailsDto.FrontUrl;
                    book[0].BackUrl = bookdetailsDto.BackUrl;
                    book[0].LeftUrl = bookdetailsDto.LeftUrl;
                    book[0].RightUrl = bookdetailsDto.RightUrl;
                    output[0].Quantity = bookdetailsDto.Quantity;
                    output[0].ItemPrice = bookdetailsDto.Price;
                    output[0].UpdatedBy = bookdetailsDto.UpdatedBy;
                    output[0].UpdatedOn = bookdetailsDto.UpdatedOn;
                    output[0].Active = bookdetailsDto.Active;
                    _context.SaveChanges();
                    transaction.Commit();

                }
            }

            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "DBConcurrency Error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }
        }

        /*
        *  function to display autosuggestions that enhances search functionality
        */
        public List<string> autosuggest(string input)
        {
            _logger.LogInformation("Preparing autosuggestions");

            List<string> names = new List<string>();

           var booker = (from booke in _context.Book join price in _context.Price on booke.Book_Id equals price.Book.Book_Id where booke.Name.ToLower().Contains(input.ToLower()) select new BookDetailsDto { BookId = booke.Book_Id, BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre, BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity, FrontUrl = booke.FrontUrl, BackUrl = booke.BackUrl, LeftUrl = booke.LeftUrl, RightUrl = booke.RightUrl }).ToList();


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




        public List<string> ScreenInventory()
        {
            List <string> BookList = new List<string>();
            var query = _context.Price
                                       .Include(price => price.Condition)
                                       .Include(price => price.Book)
                                       .Include(p => p.Book.Publisher)
                                       .Include(p => p.Book.Genre)
                                       .Include(p => p.Book.Type).ToList();
            foreach(var i in query)
            {
                if (i.Quantity <= 5)
                {
                    var detail = i.Book.Name + "*" + i.Book.Publisher.Name + "*" + i.Book.Type.TypeName + "*" + i.Condition.ConditionName + "*" + i.Quantity;
                    BookList.Add(detail);
                }
            }

            return BookList;
        }

    }


}



