using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amazon.Polly;
using DataAccess.Data;
using DataAccess.Dtos;
using DataAccess.Repository.Interface;
using DataAccess.Repository.Interface.Implementations;
using DataAccess.Repository.Interface.InventoryInterface;
using DataAccess.Repository.Interface.SearchImplementations;
using DataModels.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Type = DataModels.Books.Type;

namespace DataAccess.Repository.Implementation.InventoryImplementation
{
    public class Inventory : IInventory
    {
        private readonly int _booksPerPage = 15;
        private readonly IGenericRepository<Condition> _conditionRepository;
        private readonly IGenericRepository<Genre> _genreRepository;
        private readonly ILogger<Inventory> _logger;
        private readonly IGenericRepository<Publisher> _publisherRepository;
        private readonly IRekognitionNPollyRepository _rekognitionNPollyRepository;
        private readonly ISearchRepository _searchRepo;
        private readonly IGenericRepository<Type> _typeRepository;
        private readonly string[] PriceIncludes = { "Book", "Condition" };
        public ApplicationDbContext _context;

        public Inventory(IGenericRepository<Condition> conditionRepository,
            IGenericRepository<Type> typeRepository,
            IGenericRepository<Genre> genreRepository,
            IGenericRepository<Publisher> publisherRepository,
            ApplicationDbContext context,
            ISearchRepository searchRepository,
            IRekognitionNPollyRepository rekognitionPollyRepository,
            ILogger<Inventory> logger)
        {
            _context = context;
            _searchRepo = searchRepository;
            _rekognitionNPollyRepository = rekognitionPollyRepository;
            _logger = logger;
            _publisherRepository = publisherRepository;
            _genreRepository = genreRepository;
            _typeRepository = typeRepository;
            _conditionRepository = conditionRepository;
        }

        public BookDetailsDto GetBookByID(long bookId)
        {
            var book = (from booke in _context.Book
                join price in _context.Price on booke.Book_Id equals price.Book.Book_Id
                where booke.Book_Id == bookId
                select new BookDetailsDto
                {
                    Summary = booke.Summary, ISBN = booke.ISBN, Author = booke.Author, BookId = booke.Book_Id,
                    BookName = booke.Name, Price = price.ItemPrice, Publisher = booke.Publisher, Genre = booke.Genre,
                    BookCondition = price.Condition, BookType = booke.Type, Quantity = price.Quantity,
                    FrontUrl = booke.FrontUrl, BackUrl = booke.BackUrl, LeftUrl = booke.LeftUrl,
                    RightUrl = booke.RightUrl
                }).ToList();

            if (book.Count > 0) return book[0];

            return null;
        }

        // Add details to the Book table
        public void SaveBook(Book book)
        {
            _logger.LogInformation("Posting details to Books table");
            _context.Book.Add(book);
            _context.SaveChanges();
        }

        // Add details to the Price table
        public void SavePrice(Price price)
        {
            _logger.LogInformation("Posting details to Price table");
            _context.Price.Add(price);
            _context.SaveChanges();
        }

        // Add  details to the Publisher table
        public bool AddPublishers(Publisher publishers)
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

        // Add  details to the Genres table
        public bool AddGenres(Genre genres)
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

        // Add details to the Types table
        public bool AddBookTypes(Type booktype)
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

        // Add details to the Conditions table
        public bool AddBookConditions(Condition bookCondition)
        {
            _logger.LogInformation("Posting details to the Conditions table");

            var conditionName = _context.Condition
                .Where(condition => condition.ConditionName == bookCondition.ConditionName).ToList();

            if (conditionName.Count == 0)
            {
                _context.Condition.Add(bookCondition);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public void EditPublisher(string oldPublisherName, string newPublisherName)
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
                var genreName = _context.Genre.Where(x => x.Name == oldGenre).ToList();
                using (var transaction = _context.Database.BeginTransaction())
                {
                    genreName[0].Name = newGenre;
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

        // Fetch all publisher details for dynamic display in the drop down list 
        public List<Publisher> GetAllPublishers()
        {
            var publishers = _context.Publisher.ToList();
            return publishers;
        }


        // Fetch all Genres details for dynamic display in the drop down list 
        public List<Genre> GetGenres()
        {
            var genres = _context.Genre.ToList();
            return genres;
        }

        // Fetch all Types details for dynamic display in the drop down list 
        public List<Type> GetTypes()
        {
            var typelist = _context.Type.ToList();
            return typelist;
        }

        // Fetch all Conditions for dynamic display in the drop down list 
        public List<Condition> GetConditions()
        {
            var conditions = _context.Condition.ToList();
            return conditions;
        }

        // Fetch Book details given its BookId and Condition 
        public BookDetailsDto GetBookDetails(long bookId, long priceId)
        {
            var booker = (from book in _context.Book
                join price in _context.Price on book.Book_Id equals price.Book.Book_Id
                where book.Book_Id == bookId && price.Price_Id == priceId
                select new BookDetailsDto
                {
                    BookName = book.Name,
                    Price = price.ItemPrice,
                    Publisher = book.Publisher,
                    Genre = book.Genre,
                    BookCondition = price.Condition,
                    BookType = book.Type,
                    Quantity = price.Quantity,
                    FrontUrl = book.FrontUrl,
                    BackUrl = book.BackUrl,
                    LeftUrl = book.LeftUrl,
                    RightUrl = book.RightUrl
                }).ToList();

            return booker[0];
        }

        // Fetch all process form inputs and add details to the database 
        public async Task<bool> AddToTablesAsync(BooksDto booksDto)
        {
            _logger.LogInformation("Processing and Posting data to tables and cloud ");

            try
            {
                string frontUrl = "", backUrl = "", leftUrl = "", rightUrl = "", audioBookUrl = "";
                if (booksDto.FrontPhoto != null)
                    frontUrl = await _rekognitionNPollyRepository
                        .UploadtoS3Async(booksDto.FrontPhoto, booksDto.BookId, booksDto.BookCondition);

                if (booksDto.BackPhoto != null)
                    backUrl = await _rekognitionNPollyRepository
                        .UploadtoS3Async(booksDto.BackPhoto, booksDto.BookId, booksDto.BookCondition);

                if (booksDto.LeftSidePhoto != null)
                    leftUrl = await _rekognitionNPollyRepository
                        .UploadtoS3Async(booksDto.LeftSidePhoto, booksDto.BookId, booksDto.BookCondition);

                if (booksDto.RightSidePhoto != null)
                    rightUrl = await _rekognitionNPollyRepository
                        .UploadtoS3Async(booksDto.RightSidePhoto, booksDto.BookId, booksDto.BookCondition);

                if (_rekognitionNPollyRepository.IsContentViolation(frontUrl)
                    || _rekognitionNPollyRepository.IsContentViolation(backUrl)
                    || _rekognitionNPollyRepository.IsContentViolation(leftUrl)
                    || _rekognitionNPollyRepository.IsContentViolation(rightUrl))
                    return false;

                if (booksDto.Summary != null)
                    audioBookUrl = _rekognitionNPollyRepository.GenerateAudioSummary(booksDto.BookName,
                        booksDto.Summary, Constants.TextToSpeechLanguageCode, VoiceId.Emma);

                var book = new Book();
                var price = new Price();

                var publisherData = _context.Publisher
                    .FirstOrDefault(publisher => publisher.Name == booksDto.PublisherName);
                if (publisherData == null)
                {
                    publisherData = new Publisher
                    {
                        Name = booksDto.PublisherName
                    };
                    _publisherRepository.Add(publisherData);
                    _publisherRepository.Save();
                }

                var genreData = _context.Genre.FirstOrDefault(genre => genre.Name == booksDto.Genre);
                if (genreData == null)
                {
                    genreData = new Genre
                    {
                        Name = booksDto.Genre
                    };
                    _genreRepository.Add(genreData);
                    _genreRepository.Save();
                }

                var typeData = _typeRepository.Get(type => type.TypeName == booksDto.BookType).FirstOrDefault();
                if (typeData == null)
                {
                    typeData = new Type
                    {
                        TypeName = booksDto.BookType
                    };
                    _typeRepository.Add(typeData);
                    _typeRepository.Save();
                }

                var conditionData = _context.Condition.FirstOrDefault(condition => condition.ConditionName == booksDto.BookCondition);
                if (conditionData == null)
                {
                    conditionData = new Condition
                    {
                        ConditionName = booksDto.PublisherName
                    };
                    _conditionRepository.Add(conditionData);
                    _conditionRepository.Save();
                }

                book.Name = booksDto.BookName;
                book.Type = typeData;
                book.Genre = genreData;
                book.ISBN = booksDto.ISBN;
                book.Publisher = publisherData;
                book.FrontUrl = frontUrl;
                book.BackUrl = backUrl;
                book.LeftUrl = leftUrl;
                book.RightUrl = rightUrl;
                book.Summary = booksDto.Summary;
                book.AudioBookUrl = audioBookUrl;
                book.Author = booksDto.Author;

                price.Condition = conditionData;
                price.ItemPrice = booksDto.Price;
                price.Book = book;
                price.Quantity = booksDto.Quantity;
                price.UpdatedBy = booksDto.UpdatedBy;
                price.UpdatedOn = booksDto.UpdatedOn;
                price.Active = booksDto.Active;

                var books = _context.Book.Where(temp =>
                    temp.Name == book.Name && temp.Type == book.Type && temp.Publisher == book.Publisher &&
                    temp.Genre == book.Genre).ToList();
                if (books.Count == 0)
                {
                    SaveBook(book);
                    SavePrice(price);
                }
                else
                {
                    price.Book = books[0];
                    var prices = _context.Price.Where(p => p.Condition == price.Condition && p.Book.Name == book.Name)
                        .ToList();
                    if (prices.Count == 0)
                    {
                        SavePrice(price);
                    }
                    else
                    {
                        var output = _context.Price
                            .Where(p => p.Condition == price.Condition && p.Book.Name == book.Name).ToList();
                        output[0].Quantity = booksDto.Quantity;
                        output[0].ItemPrice = booksDto.Price;
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in AddBooksFunc");
            }

            return true;
        }

        public IEnumerable<BookDetailsDto> GetDetails(long bookId)
        {
            var booker = (from booke in _context.Book
                join price in _context.Price on booke.Book_Id equals price.Book.Book_Id
                where booke.Book_Id == bookId
                select new BookDetailsDto
                {
                    BookId = booke.Book_Id,
                    BookName = booke.Name,
                    Price = price.ItemPrice,
                    Publisher = booke.Publisher,
                    Genre = booke.Genre,
                    BookCondition = price.Condition,
                    BookType = booke.Type,
                    Quantity = price.Quantity,
                    FrontUrl = booke.FrontUrl,
                    BackUrl = booke.BackUrl,
                    LeftUrl = booke.LeftUrl,
                    RightUrl = booke.RightUrl
                }).ToList();

            return booker;
        }

        public SearchBookDto GetAllBooks(int pageNum, string style, string sortBy, string ascDesc)
        {
            var query = (IQueryable<Price>)_context.Price;

            query = query.Include(PriceIncludes);

            var booksList = RetrieveUniqueBookList(query);

            var finalBooksList = RetrieveSortedBookList(booksList, sortBy, ascDesc);

            var totalPages = _searchRepo.GetTotalPages(finalBooksList.Count(), _booksPerPage);

            var viewModel = new SearchBookDto();

            viewModel = RetrieveFilterDto(finalBooksList, ascDesc, style, totalPages, pageNum, "", "");

            return viewModel;
        }

        public SearchBookDto SearchBooks(string searchBy, string searchFilter, string style, string sortBy, int pageNum,
            string ascDesc)
        {
            searchBy = " " + searchBy;
            var fullyQualifiedName = typeof(DataModels.Books.Price).AssemblyQualifiedName;
            var parameterExpression =
                Expression.Parameter(System.Type.GetType(fullyQualifiedName), "order");

            var expression = _searchRepo.ReturnExpression(parameterExpression, searchBy, searchFilter);

            searchBy = searchBy.Trim();
            var lambda = Expression.Lambda<Func<Price, bool>>(expression, parameterExpression);

            if (lambda == null)
            {
                var pages = Enumerable.Range(1, 1).ToArray();

                return RetrieveDto(ascDesc, style, "", "", 1, 1, pages, null);
            }

            var query = _context.Price.Include(PriceIncludes);
            

            var filterQuery = query.Where(lambda);

            var booksList = RetrieveUniqueBookList(filterQuery);

            var finalBooksList = RetrieveSortedBookList(booksList, sortBy, ascDesc);

            var totalPages = _searchRepo.GetTotalPages(finalBooksList.Count(), _booksPerPage);

            var searchBookDto = RetrieveFilterDto(finalBooksList, ascDesc, style, totalPages, pageNum, searchBy,
                searchFilter);

            return searchBookDto;
        }

        public List<string> GetFormatsOfTheSelectedBook(string bookName)
        {
            _logger.LogInformation("Fetching all possible formats of the given book");

            var types = new List<string>();
            var book = (from booke in _context.Book
                join price in _context.Price on booke.Book_Id equals price.Book.Book_Id
                where booke.Name == bookName
                select new BookDetailsDto
                {
                    BookId = booke.Book_Id,
                    BookName = booke.Name,
                    Price = price.ItemPrice,
                    Publisher = booke.Publisher,
                    Genre = booke.Genre,
                    BookCondition = price.Condition,
                    BookType = booke.Type,
                    Quantity = price.Quantity,
                    FrontUrl = booke.FrontUrl,
                    BackUrl = booke.BackUrl,
                    LeftUrl = booke.LeftUrl,
                    RightUrl = booke.RightUrl
                }).ToList();

            foreach (var i in book)
                if (!types.Contains(i.BookType.TypeName))
                    types.Add(i.BookType.TypeName);

            return types;
        }

        public List<string> GetConditionsOfTheSelectedBook(string bookName)
        {
            _logger.LogInformation("Fetching all possible conditions of the given book");

            var conditions = new List<string>();
            var book = (from booke in _context.Book
                join price in _context.Price on booke.Book_Id equals price.Book.Book_Id
                where booke.Name == bookName
                select new BookDetailsDto
                {
                    BookId = booke.Book_Id,
                    BookName = booke.Name,
                    Price = price.ItemPrice,
                    Publisher = booke.Publisher,
                    Genre = booke.Genre,
                    BookCondition = price.Condition,
                    BookType = booke.Type,
                    Quantity = price.Quantity,
                    FrontUrl = booke.FrontUrl,
                    BackUrl = booke.BackUrl,
                    LeftUrl = booke.LeftUrl,
                    RightUrl = booke.RightUrl
                }).ToList();

            foreach (var i in book)
                if (!conditions.Contains(i.BookCondition.ConditionName))
                    conditions.Add(i.BookCondition.ConditionName);

            return conditions;
        }

        public List<BookDetailsDto> GetRelevantBooks(string bookName, string type, string conditionChosen)
        {
            _logger.LogInformation("Fetching all possible books based on type and condition chosen");

            if (!string.IsNullOrEmpty(conditionChosen) && !string.IsNullOrEmpty(type))
            {
                var book = (from booke in _context.Book
                    join price in _context.Price on booke.Book_Id equals price.Book.Book_Id
                    where booke.Name == bookName
                          && booke.Type.TypeName == type
                          && price.Condition.ConditionName == conditionChosen
                    select new BookDetailsDto
                    {
                        Author = booke.Author,
                        BookId = booke.Book_Id,
                        BookName = booke.Name,
                        Price = price.ItemPrice,
                        Publisher = booke.Publisher,
                        Genre = booke.Genre,
                        BookCondition = price.Condition,
                        BookType = booke.Type,
                        Quantity = price.Quantity,
                        FrontUrl = booke.FrontUrl,
                        BackUrl = booke.BackUrl,
                        LeftUrl = booke.LeftUrl,
                        RightUrl = booke.RightUrl
                    }).ToList();

                return book;
            }

            if (!string.IsNullOrEmpty(conditionChosen))
            {
                var book = (from booke in _context.Book
                    join price in _context.Price on booke.Book_Id equals price.Book.Book_Id
                    where booke.Name == bookName
                          && price.Condition.ConditionName == conditionChosen
                    select new BookDetailsDto
                    {
                        BookId = booke.Book_Id,
                        BookName = booke.Name,
                        Price = price.ItemPrice,
                        Publisher = booke.Publisher,
                        Genre = booke.Genre,
                        BookCondition = price.Condition,
                        BookType = booke.Type,
                        Quantity = price.Quantity,
                        FrontUrl = booke.FrontUrl,
                        BackUrl = booke.BackUrl,
                        LeftUrl = booke.LeftUrl,
                        RightUrl = booke.RightUrl,
                        Author = booke.Author
                    }).ToList();

                return book;
            }

            if (!string.IsNullOrEmpty(type))
            {
                var book = (from booke in _context.Book
                    join price in _context.Price on booke.Book_Id equals price.Book.Book_Id
                    where booke.Name == bookName
                          && price.Condition.ConditionName == conditionChosen
                    select new BookDetailsDto
                    {
                        BookId = booke.Book_Id,
                        BookName = booke.Name,
                        Price = price.ItemPrice,
                        Publisher = booke.Publisher,
                        Genre = booke.Genre,
                        BookCondition = price.Condition,
                        BookType = booke.Type,
                        Quantity = price.Quantity,
                        FrontUrl = booke.FrontUrl,
                        BackUrl = booke.BackUrl,
                        LeftUrl = booke.LeftUrl,
                        RightUrl = booke.RightUrl,
                        Author = booke.Author
                    }).ToList();

                return book;
            }

            var booker = (from booke in _context.Book
                join price in _context.Price on booke.Book_Id equals price.Book.Book_Id
                where booke.Name == bookName
                select new BookDetailsDto
                {
                    BookId = booke.Book_Id,
                    BookName = booke.Name,
                    Price = price.ItemPrice,
                    Publisher = booke.Publisher,
                    Genre = booke.Genre,
                    BookCondition = price.Condition,
                    BookType = booke.Type,
                    Quantity = price.Quantity,
                    FrontUrl = booke.FrontUrl,
                    BackUrl = booke.BackUrl,
                    LeftUrl = booke.LeftUrl,
                    RightUrl = booke.RightUrl,
                    Author = booke.Author
                }).ToList();

            return booker;
        }

        public List<Dictionary<string, int>> DashBoard(int numberOfDetails)
        {
            _logger.LogInformation("Packing together dashboard results ");

            var topfive = topStatsForOrders(numberOfDetails);
            var Inventory = InventoryStats();
            var Orders = OrderStats();

            topfive.Add(Inventory);
            topfive.Add(Orders);

            return topfive;
        }

        // Update details of an existing book
        public BookDetailsDto UpdateDetails(int id, string condition)
        {
            _logger.LogInformation("Fetching data to pre-populate edit page ");

            var list = (from booke in _context.Book
                join price in _context.Price on booke.Book_Id equals price.Book.Book_Id
                where booke.Book_Id == id && price.Condition.ConditionName == condition
                select new BookDetailsDto
                {
                    Author = booke.Author,
                    Active = price.Active,
                    BookId = booke.Book_Id,
                    BookName = booke.Name,
                    Price = price.ItemPrice,
                    Publisher = booke.Publisher,
                    Genre = booke.Genre,
                    BookCondition = price.Condition,
                    BookType = booke.Type,
                    Quantity = price.Quantity,
                    FrontUrl = booke.FrontUrl,
                    BackUrl = booke.BackUrl,
                    LeftUrl = booke.LeftUrl,
                    RightUrl = booke.RightUrl
                }).ToList();

            return list[0];
        }

        // Update details of an existing book
        public async Task PushDetailsAsync(BookDetailsDto bookDetailsDto)
        {
            _logger.LogInformation("Pushing edited details to database");

            try
            {
                var output = _context.Price.Where(p =>
                    p.Condition.ConditionName == bookDetailsDto.BookCondition.ConditionName &&
                    p.Book.Book_Id == bookDetailsDto.BookId).ToList();

                if (bookDetailsDto.FrontPhoto != null)
                    bookDetailsDto.FrontUrl = await _rekognitionNPollyRepository.UploadtoS3Async(bookDetailsDto.FrontPhoto,
                        bookDetailsDto.BookId, bookDetailsDto.BookCondition.ConditionName);

                if (bookDetailsDto.BackPhoto != null)
                    bookDetailsDto.BackUrl = await _rekognitionNPollyRepository.UploadtoS3Async(bookDetailsDto.BackPhoto,
                        bookDetailsDto.BookId, bookDetailsDto.BookCondition.ConditionName);

                if (bookDetailsDto.LeftSidePhoto != null)
                    bookDetailsDto.LeftUrl = await _rekognitionNPollyRepository.UploadtoS3Async(bookDetailsDto.LeftSidePhoto,
                        bookDetailsDto.BookId, bookDetailsDto.BookCondition.ConditionName);

                if (bookDetailsDto.RightSidePhoto != null)
                    bookDetailsDto.RightUrl = await _rekognitionNPollyRepository.UploadtoS3Async(bookDetailsDto.RightSidePhoto,
                        bookDetailsDto.BookId, bookDetailsDto.BookCondition.ConditionName);

                await using var transaction = await _context.Database.BeginTransactionAsync();

                var book = _context.Book.Where(p => p.Book_Id == bookDetailsDto.BookId).ToList();
                book[0].FrontUrl = bookDetailsDto.FrontUrl;
                book[0].BackUrl = bookDetailsDto.BackUrl;
                book[0].LeftUrl = bookDetailsDto.LeftUrl;
                book[0].RightUrl = bookDetailsDto.RightUrl;
                output[0].Quantity = bookDetailsDto.Quantity;
                output[0].ItemPrice = bookDetailsDto.Price;
                output[0].UpdatedBy = bookDetailsDto.UpdatedBy;
                output[0].UpdatedOn = bookDetailsDto.UpdatedOn;
                output[0].Active = bookDetailsDto.Active;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
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

        // Display autosuggestions that enhances search functionality
        public List<string> AutoSuggest(string input)
        {
            _logger.LogInformation("Preparing autosuggestions");

            var names = new List<string>();

            var booker = (from booke in _context.Book
                join price in _context.Price on booke.Book_Id equals price.Book.Book_Id
                where booke.Name.ToLower().Contains(input.ToLower())
                select new BookDetailsDto
                {
                    BookId = booke.Book_Id,
                    BookName = booke.Name,
                    Price = price.ItemPrice,
                    Publisher = booke.Publisher,
                    Genre = booke.Genre,
                    BookCondition = price.Condition,
                    BookType = booke.Type,
                    Quantity = price.Quantity,
                    FrontUrl = booke.FrontUrl,
                    BackUrl = booke.BackUrl,
                    LeftUrl = booke.LeftUrl,
                    RightUrl = booke.RightUrl
                }).ToList();

            foreach (var i in booker)
                if (!names.Contains(i.BookName))
                    names.Add(i.BookName);

            foreach (var i in GetGenres())
                if (i.Name.ToLower().Contains(input.ToLower()))
                    names.Add(i.Name);

            foreach (var i in GetAllPublishers())
                if (i.Name.ToLower().Contains(input.ToLower()))
                    names.Add(i.Name);

            foreach (var i in GetTypes())
                if (i.TypeName.ToLower().Contains(input.ToLower()))
                    names.Add(i.TypeName);
            foreach (var i in GetConditions())
                if (i.ConditionName.ToLower().Contains(input.ToLower()))
                    names.Add(i.ConditionName);

            return names;
        }

        public List<string> ScreenInventory()
        {
            var bookList = new List<string>();
            var query = _context.Price
                .Include(price => price.Condition)
                .Include(price => price.Book)
                .Include(p => p.Book.Publisher)
                .Include(p => p.Book.Genre)
                .Include(p => p.Book.Type).ToList();

            foreach (var i in query)
                if (i.Quantity <= 5)
                {
                    var detail = i.Book.Name + "*" + i.Book.Publisher.Name + "*" + i.Book.Type.TypeName + "*" +
                                 i.Condition.ConditionName + "*" + i.Quantity;
                    bookList.Add(detail);
                }

            return bookList;
        }

        private IQueryable<Price> GetBaseOrderQuery()
        {
            var query = _context.Price
                .Include(price => price.Condition)
                .Include(price => price.Book);
            return query;
        }

        private List<FullBookDto> RetrieveUniqueBookList(IQueryable<Price> query)
        {
            List<Price> books = null;

            var bookIds = new List<string>();
            var finalBooksList = new List<FullBookDto>();

            books = query.ToList();
            foreach (var book in books)
                if (!bookIds.Contains(book.Book.Name))
                {
                    bookIds.Add(book.Book.Name);
                    finalBooksList.Add(new FullBookDto
                        { LowestPrice = (int)book.ItemPrice, TotalQuantity = book.Quantity, Price = book });
                }
                else
                {
                    var fullBook = finalBooksList.Where(find => find.Price.Book.Name == book.Book.Name)
                        .FirstOrDefault();
                    fullBook.TotalQuantity += book.Quantity;
                    fullBook.LowestPrice = fullBook.LowestPrice > (int)book.ItemPrice
                        ? (int)book.ItemPrice
                        : fullBook.LowestPrice;
                }

            return finalBooksList;
        }

        private List<FullBookDto> RetrieveSortedBookList(List<FullBookDto> finalBooksList, string sortBy,
            string ascDesc)
        {
            var parameterExpression2 =
                Expression.Parameter(System.Type.GetType("DataAccess.Dtos.FullBookDto"), "fullbook");

            Expression property = null;

            if (string.IsNullOrEmpty(sortBy))
            {
                return finalBooksList;
            }

            if (!sortBy.Contains("."))
            {
                property = Expression.Property(parameterExpression2, sortBy);

                var _sortBy = Expression.Lambda<Func<FullBookDto, int>>(property, parameterExpression2);

                if (string.IsNullOrEmpty(ascDesc)) ascDesc = "asc";

                var bookList = ascDesc.Contains("asc")
                    ? finalBooksList.AsQueryable().OrderBy(_sortBy)
                    : finalBooksList.AsQueryable().OrderByDescending(_sortBy);

                return bookList.ToList();
            }
            else
            {
                property = parameterExpression2;
                foreach (var member in sortBy.Split('.')) property = Expression.PropertyOrField(property, member);

                var _sortBy = Expression.Lambda<Func<FullBookDto, string>>(property, parameterExpression2);

                if (string.IsNullOrEmpty(ascDesc)) ascDesc = "asc";

                var bookList = ascDesc.Contains("asc")
                    ? finalBooksList.AsQueryable().OrderBy(_sortBy)
                    : finalBooksList.AsQueryable().OrderByDescending(_sortBy);

                return bookList.ToList();
            }
        }

        private SearchBookDto RetrieveDto(string ascDesc, string style, string filterValue, string searchString,
            int pageNum, int totalPages, int[] pages, List<FullBookDto> books)
        {
            var viewModel = new SearchBookDto
            {
                Searchby = filterValue,
                Searchfilter = searchString,
                Books = books,
                Pages = pages,
                HasPreviousPages = pageNum > 1,
                CurrentPage = pageNum,
                HasNextPages = pageNum < totalPages,
                Ascdesc = ascDesc,
                ViewStyle = style
            };

            return viewModel;
        }

        private SearchBookDto RetrieveFilterDto(List<FullBookDto> filterQuery, string ascDesc, string style,
            int totalPages, int pageNum, string filterValue, string searchString)
        {
            var viewModel = new SearchBookDto();

            var books = filterQuery
                .Skip((pageNum - 1) * _booksPerPage)
                .Take(_booksPerPage)
                .ToList();

            var pages = _searchRepo.GetModifiedPagesArr(pageNum, totalPages);

            var searchBookDto =
                RetrieveDto(ascDesc, style, filterValue, searchString, pageNum, totalPages, pages, books);

            return searchBookDto;
        }

        // Fetch and calculate dashboard statistics 
        private List<Dictionary<string, int>> topStatsForOrders(int numberOfDetails)
        {
            _logger.LogInformation("calculating Top stats for orders");

            var genre_stats = new Dictionary<string, int>();
            var publisher_stats = new Dictionary<string, int>();
            var condition_stats = new Dictionary<string, int>();
            var bookname_stats = new Dictionary<string, int>();
            var type_stats = new Dictionary<string, int>();
            var topStats = new List<Dictionary<string, int>>();

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

        private Dictionary<string, int> InventoryStats()
        {
            _logger.LogInformation("calculating Inventory stats");

            var totalMerchandiseValue = 0;
            var totalBooksInventory = 0;

            var count_stats = new Dictionary<string, int>();

            try
            {
                var books = GetBaseOrderQuery().ToList();
                foreach (var i in books)
                {
                    totalBooksInventory = totalBooksInventory + i.Quantity;
                    totalMerchandiseValue = totalMerchandiseValue + (int)i.ItemPrice * i.Quantity;
                }

                var total_titles = _context.Book.ToList().Count();

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
            var total_quantity_sold = 0;

            var count_stats = new Dictionary<string, int>();

            var delivered = _context.Order.Where(x => x.OrderStatus.Status == Constants.OrderStatusDelivered).ToList()
                .Count();
            var justplaced = _context.Order.Where(x => x.OrderStatus.Status == Constants.OrderStatusJustPlaced).ToList()
                .Count();
            var enroute = _context.Order.Where(x => x.OrderStatus.Status == Constants.OrderStatusEnRoute).ToList()
                .Count();
            var pending = _context.Order.Where(x => x.OrderStatus.Status == Constants.OrderStatusPending).ToList()
                .Count();
            var ordersplaced = delivered + justplaced + enroute + pending;

            var orderdetailsList = _context.OrderDetail.ToList();
            foreach (var i in orderdetailsList)
            {
                total_quantity_sold = total_quantity_sold + i.Quantity;
                total_sales_value = total_sales_value + i.OrderDetailPrice * i.Quantity;
            }

            count_stats["Orders_placed"] = ordersplaced;
            count_stats[Constants.OrderStatusDelivered] = delivered;
            count_stats[Constants.OrderStatusEnRoute] = enroute;
            count_stats[Constants.OrderStatusPending] = pending;
            count_stats[Constants.OrderStatusJustPlaced] = justplaced;
            count_stats["total_books_sold"] = total_quantity_sold;
            count_stats["total_sales"] = (int)total_sales_value;

            Console.WriteLine("Error in Fetching Order Stats");

            return count_stats;
        }
    }
}
