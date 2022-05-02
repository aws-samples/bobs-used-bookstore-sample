using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Amazon.Extensions.CognitoAuthentication;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.DataAccess.Dtos;
using BobsBookstore.DataAccess.Repository.Interface;
using BobsBookstore.DataAccess.Repository.Interface.InventoryInterface;
using BobsBookstore.Models.Books;
using BookstoreBackend.Notifications.NotificationsInterface;
using BookstoreBackend.ViewModel;
using BookstoreBackend.ViewModel.ManageInventory;
using BookstoreBackend.ViewModel.SearchBooks;
using Type = BobsBookstore.Models.Books.Type;

namespace BookstoreBackend.Controllers
{
    public class InventoryController : Controller
    {
        private const int NumberOfDetails = 5;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly INotifications _emailSender;
        private readonly IInventory _inventory;
        private readonly ILogger<InventoryController> _logger;
        private readonly IMapper _mapper;
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;

        public InventoryController(IGenericRepository<Book> bookRepository,
                                   IMapper mapper,
                                   IInventory inventory,
                                   ApplicationDbContext context,
                                   ILogger<InventoryController> logger,
                                   INotifications emailSender,
                                   SignInManager<CognitoUser> signInManager,
                                   UserManager<CognitoUser> userManager)
        {
            _inventory = inventory;
            _logger = logger;
            _emailSender = emailSender;
            _mapper = mapper;
            _bookRepository = bookRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> EditBookDetails(string bookName)
        {
            _logger.LogInformation("Loading : Add New Book View");
            try
            {
                var user = await _userManager.GetUserAsync(User);
                ViewData["user"] = user.Username;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in authentication @ Adding a new Book");
            }

            try
            {
                if (!string.IsNullOrEmpty(bookName))
                {
                    var temp = _bookRepository.Get(b => b.Name == bookName).FirstOrDefault();
                    var variant = _inventory.GetBookByID(temp.Book_Id);
                    ViewData["ISBN"] = variant.ISBN;
                    ViewData["Author"] = variant.Author;
                    ViewData["Summary"] = variant.Summary;
                }

                if (bookName != null)
                    ViewData["Book"] = bookName;

                ViewData["Types"] = _inventory.GetTypes();
                ViewData["Publishers"] = _inventory.GetAllPublishers();
                ViewData["Genres"] = _inventory.GetGenres();
                ViewData["Conditions"] = _inventory.GetConditions();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in loading data for Books Action Method");
            }

            ViewData["check"] = Constants.ErrorStatusYes;

            return View();
        }

        [HttpPost] //Post Data from forms to tables
        public async Task<IActionResult> EditBookDetails(BooksViewModel bookview)
        {
            _logger.LogInformation("Posting new book details from form to Database ");
            var user = await _userManager.GetUserAsync(User);
            bookview.UpdatedBy = user.Username;
            bookview.UpdatedOn = DateTime.Now.ToUniversalTime();
            bookview.Active = true;
            if (string.IsNullOrEmpty(bookview.Author))
            {
                var temp = _bookRepository.Get(b => b.Name == bookview.BookName).FirstOrDefault();
                var variant = _inventory.GetBookByID(temp.Book_Id);
                bookview.Author = variant.Author;
                bookview.Summary = variant.Summary;
            }

            if (ModelState.IsValid)
            {
                var booksDto = _mapper.Map<BooksDto>(bookview);
                var status = _inventory.AddToTables(booksDto);

                if (!status)
                {
                    ViewData["ErrorStatus"] = Constants.ErrorStatusYes;

                    ViewData["Types"] = _inventory.GetTypes();
                    ViewData["Publishers"] = _inventory.GetAllPublishers();
                    ViewData["Genres"] = _inventory.GetGenres();
                    ViewData["Conditions"] = _inventory.GetConditions();
                    return View(bookview);
                }

                var temp = _bookRepository.Get(b => b.Name == bookview.BookName).FirstOrDefault();
                var bookId = temp.Book_Id;
                return RedirectToAction("BookDetails", new { BookId = bookId });
            }

            return View(bookview);
        }

        [HttpGet]
        public IActionResult AddPublishers()
        {
            ViewData["Status"] = Constants.AddPublisherMessage;
            ViewData["Publishers"] = _inventory.GetAllPublishers();

            return View();
        }

        [HttpPost]
        public IActionResult AddPublishers(Publisher publisher, string source)
        {
            try
            {
                var status = _inventory.AddPublishers(publisher);

                if (status)
                    return RedirectToAction("EditBookDetails");
                ViewData["Status"] = Constants.PublisherExistsStatus;
            }

            catch (Exception e)
            {
                _logger.LogError(e, "Error in adding a new publisher");
            }

            return View();
        }

        [HttpGet]
        public IActionResult AddGenres()
        {
            ViewData["Status"] = Constants.AddGenreMessage;
            return View();
        }

        [HttpPost]
        public IActionResult AddGenres(Genre genre)
        {
            try
            {
                var status = _inventory.AddGenres(genre);

                if (status)
                    return RedirectToAction("EditBookDetails");
                ViewData["Status"] = Constants.GenreExistsStatus;
            }

            catch (Exception e)
            {
                _logger.LogError(e, "Error in adding a new genre");
            }

            return View();
        }

        [HttpGet]
        public IActionResult AddBookTypes()
        {
            ViewData["Status"] = Constants.AddTypeMessage;
            return View();
        }

        [HttpPost]
        public IActionResult AddBookTypes(Type booktype)
        {
            try
            {
                var status = _inventory.AddBookTypes(booktype);

                if (status)
                    return RedirectToAction("EditBookDetails");
                ViewData["Status"] = Constants.TypeExistsStatus;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in adding a new type");
            }

            return View();
        }

        [HttpGet]
        public IActionResult AddBookConditions()
        {
            ViewData["Status"] = Constants.AddConditionsMessage;
            return View();
        }

        [HttpPost]
        public IActionResult AddBookConditions(Condition bookcondition)
        {
            try
            {
                var status = _inventory.AddBookConditions(bookcondition);

                if (status)
                    return RedirectToAction("EditBookDetails");
                ViewData["Status"] = Constants.ConditionExistsStatus;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in adding a new condition");
            }

            return View();
        }

        [HttpGet]
        public IActionResult EditPublisher()
        {
            ViewData["Publishers"] = _inventory.GetAllPublishers();
            return View();
        }


        [HttpPost]
        public IActionResult EditPublisher(string actual, string name)
        {
            if (string.CompareOrdinal(actual, name) == 0)
            {
                ViewData["Status"] = "You seem to have not made any change , Please Recheck";
            }
            else
            {
                _inventory.EditPublisher(actual, name);
                ViewData["Status"] = "Successfully updated the existing records";
            }

            ViewData["Publishers"] = _inventory.GetAllPublishers();

            return View();
        }

        [HttpGet]
        public IActionResult EditGenre()
        {
            ViewData["Genres"] = _inventory.GetGenres();
            return View();
        }

        public IActionResult EditGenre(string actual, string name)
        {
            if (string.CompareOrdinal(actual, name) == 0)
            {
                ViewData["Status"] = "Successfully updated the existing records";
            }
            else
            {
                _inventory.EditGenre(actual, name);
                ViewData["Status"] = "Successfully updated the existing records";
            }

            ViewData["Genres"] = _inventory.GetGenres();

            return View();
        }

        [HttpGet]
        public IActionResult EditCondition()
        {
            ViewData["Conditions"] = _inventory.GetConditions();
            return View();
        }

        public IActionResult EditCondition(string actual, string name)
        {
            if (string.CompareOrdinal(actual, name) == 0)
            {
                ViewData["Status"] = "Successfully updated the existing records";
            }
            else
            {
                _inventory.EditCondition(actual, name);
                ViewData["Status"] = "Successfully updated the existing records";
            }

            ViewData["Conditions"] = _inventory.GetConditions();

            return View();
        }

        [HttpGet]
        public IActionResult EditType()
        {
            ViewData["Types"] = _inventory.GetTypes();
            return View();
        }

        public IActionResult EditType(string actual, string name)
        {
            if (string.CompareOrdinal(actual, name) == 0)
            {
                ViewData["Status"] = "Successfully updated the existing records";
            }
            else
            {
                _inventory.EditType(actual, name);
                ViewData["Status"] = "Successfully updated the existing records";
            }

            ViewData["Types"] = _inventory.GetTypes();

            return View();
        }


        [HttpGet]
        public IActionResult BookDetails(long bookId)
        {
            _logger.LogInformation("Loading Book Details on Click in search page");

            var books = new FetchBooksViewModel();
            try
            {
                var bookDetailDto = _inventory.GetBookByID(bookId);
                var bookDetails =
                    _mapper.Map<IEnumerable<BookDetailsDto>, IEnumerable<BookDetailsViewModel>>(
                        _inventory.GetDetails(bookId));
                books.Publisher = bookDetailDto.Publisher.Name;
                books.Genre = bookDetailDto.Genre.Name;
                books.BookType = bookDetailDto.BookType.TypeName;
                books.BookName = bookDetailDto.BookName;
                books.Books = bookDetails;
                books.FrontUrl = bookDetailDto.FrontUrl;
                books.BackUrl = bookDetailDto.BackUrl;
                books.LeftUrl = bookDetailDto.LeftUrl;
                books.RightUrl = bookDetailDto.RightUrl;
                books.Author = bookDetailDto.Author;
                books.ISBN = bookDetailDto.ISBN;
                books.Summary = bookDetailDto.Summary;

                ViewData["Types"] = _inventory.GetFormatsOfTheSelectedBook(bookDetailDto.BookName);
                ViewData["Conditions"] = _inventory.GetConditionsOfTheSelectedBook(bookDetailDto.BookName);
                ViewData["status"] = Constants.BookDetailsStatusDetails;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in displaying Book Details");
            }

            return View(books);
        }

        [HttpPost]
        public IActionResult BookDetails(string type, string chosenCondition, string bookName, FetchBooksViewModel book)
        {
            _logger.LogInformation("Loading books to be displayed in table based on type and condition chosen");
            try
            {
                ViewData["Types"] = _inventory.GetFormatsOfTheSelectedBook(bookName);
                ViewData["Conditions"] = _inventory.GetConditionsOfTheSelectedBook(bookName);
                ViewData["status"] = "List";

                var bookDetails =
                    _mapper.Map<List<BookDetailsDto>, IEnumerable<BookDetailsViewModel>>(
                        _inventory.GetRelevantBooks(bookName, type, chosenCondition));
                ViewData["Books"] = bookDetails;
                var lis = _inventory.GetRelevantBooks(bookName, type, chosenCondition);
                if (lis.Count == 0)
                {
                    var temp = _bookRepository.Get(b => b.Name == bookName).FirstOrDefault();
                    var variant = _inventory.GetBookByID(temp.Book_Id);
                    book.Genre = variant.Genre.Name;
                    book.Author = variant.Author;
                    book.Summary = variant.Summary;
                    ViewData["status"] = Constants.BookDetailsStatusDetails;
                    ViewData["fetchstatus"] = Constants.CombinationErrorStatus;
                    return View(book);
                }

                book.BookType = lis[0].BookType.TypeName;
                book.Publisher = lis[0].Publisher.Name;
                book.Genre = lis[0].Genre.Name;
                book.FrontUrl = lis[0].FrontUrl;
                book.BackUrl = lis[0].BackUrl;
                book.Author = lis[0].Author;
                book.Author = lis[0].Author;
                book.ISBN = lis[0].ISBN;
                book.Summary = lis[0].Summary;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in loading table data for book details ");
            }

            return View(book);
        }

        public IActionResult SearchBeta(string searchfilter, string searchby, int pageNum, string viewStyle,
            string sortBy, string ascdesc, string pagination)
        {
            _logger.LogInformation("Search Page");
            try
            {
                var stats = _inventory.DashBoard(NumberOfDetails);
                ViewData["genre"] = stats[0].OrderByDescending(x => x.Value).First().Key;
                ViewData["type"] = stats[1].OrderByDescending(x => x.Value).First().Key;
                ViewData["publisher"] = stats[2].OrderByDescending(x => x.Value).First().Key;
                ViewData["name"] = stats[3].OrderByDescending(x => x.Value).First().Key;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in loading search page dashboard");
            }

            try
            {
                if (pageNum == 0) pageNum++;

                if (string.IsNullOrEmpty(searchby) || string.IsNullOrEmpty(searchfilter))
                {
                    var books = _inventory.GetAllBooks(pageNum, viewStyle, sortBy, ascdesc);
                    books.SortBy = sortBy;

                    var viewModel = _mapper.Map<SearchBookViewModel>(books);

                    return View(viewModel);
                }
                else
                {
                    var books = _inventory.SearchBooks(searchby, searchfilter, viewStyle, sortBy, pageNum, ascdesc);
                    books.SortBy = sortBy;

                    var viewModel = _mapper.Map<SearchBookViewModel>(books);

                    return View(viewModel);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in loading search page");
            }

            return View();
        }

        [HttpPost]
        public IActionResult UpdateDetails(int bookId, string condition)
        {
            _logger.LogInformation("Load Edit Book Details page with pre-filled values");
            try
            {
                var details = _inventory.UpdateDetails(bookId, condition);

                var viewModel = _mapper.Map<BookDetailsViewModel>(details);

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in fetching prefilled values for edit book details page");
            }

            return View();
        }

        public async Task<IActionResult> SubmitChangesAsync(BookDetailsDto details)
        {
            _logger.LogInformation("Posting the Edit Book form values to database");
            try
            {
                var user = await _userManager.GetUserAsync(User);
                details.UpdatedBy = user.Username;
                details.UpdatedOn = DateTime.Now.ToUniversalTime();
                _inventory.PushDetails(details);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in posting Edited Book details to database");
            }

            return RedirectToAction("SearchBeta");
        }

        public IActionResult Dashboard(FetchBooksViewModel book)
        {
            _logger.LogInformation("Dashboard Display");

            // _emailSender.SendInventoryLowEmail(_Inventory.ScreenInventory(), Constants.BoBsEmailAddress);
            try
            {
                var stats = _inventory.DashBoard(NumberOfDetails);

                if (stats[0].Count() != 0)
                {
                    ViewData["orders_top_genre"] = stats[0].First().Key;
                    ViewData["orders_top_genre_count"] = stats[0].First().Value;
                }

                if (stats[1].Count() != 0)
                {
                    ViewData["orders_top_type"] = stats[1].First().Key;
                    ViewData["orders_top_type_count"] = stats[1].First().Value;
                }

                if (stats[2].Count != 0)
                {
                    ViewData["orders_top_publisher"] = stats[2].First().Key;
                    ViewData["orders_top_publisher_count"] = stats[2].First().Value;
                }

                if (stats[1].Count() != 0)
                {
                    ViewData["orders_top_name"] = stats[3].First().Key;
                    ViewData["orders_top_name_count"] = stats[3].First().Value;
                }

                ViewData["orders_genre"] = stats[0];
                ViewData["orders_type"] = stats[1];
                ViewData["orders_publisher"] = stats[2];
                ViewData["orders_name"] = stats[3];

                var a = stats[4];

                var inventoryStats = new List<int>();
                foreach (var i in a) inventoryStats.Add(i.Value);
                ViewData["Inventory"] = inventoryStats;

                var b = stats[5];
                var ordersStats = new List<int>();
                foreach (var i in b) ordersStats.Add(i.Value);
                ViewData["Orders"] = ordersStats;
            }
            catch (Exception e)
            {
                _logger.LogError("Error in displaying dashboard statistics", e);
                return RedirectToAction("Error", "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult AutoSuggest(string searchby)
        {
            try
            {
                var term = HttpContext.Request.Query["term"].ToString();
                var names = _inventory.autosuggest(term);

                return Ok(names);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in loading autosearch results");
                return BadRequest();
            }
        }
    }
}
