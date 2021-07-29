using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BobsBookstore.DataAccess.Data;

using BobsBookstore.Models.Books;
using BookstoreBackend.Notifications.NotificationsInterface;
using BookstoreBackend.ViewModel;
using Type = BobsBookstore.Models.Books.Type;
using BobsBookstore.DataAccess.Repository.Interface.InventoryInterface;
using BobsBookstore.DataAccess.Dtos;
using BookstoreBackend.ViewModel.ManageInventory;
using AutoMapper;
using BookstoreBackend.ViewModel;
using BookstoreBackend.ViewModel.SearchBooks;
using BobsBookstore.DataAccess.Repository.Interface;

namespace BookstoreBackend.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventory _Inventory;
        private readonly ILogger<InventoryController> _logger;
        private INotifications _emailSender;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Book> _bookRepository;

        public InventoryController(IGenericRepository<Book> bookRepository, IMapper mapper, IInventory Inventory, ApplicationDbContext context, ILogger<InventoryController> logger, INotifications emailSender)
        {
            _Inventory = Inventory;
            _logger = logger;
            _emailSender = emailSender;
            _mapper = mapper;
            _bookRepository = bookRepository;

        }

        [HttpGet]
        public IActionResult EditBookDetails(string BookName)
        {
            _logger.LogInformation("Loading : Add New Book View");
            try
            {
                ViewData["user"] = @User.Claims.FirstOrDefault(c => c.Type.Equals("cognito:username"))?.Value;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in authentication @ Adding a new Book");
            }

            DateTime time = DateTime.Now;
            try
            {
                if (!string.IsNullOrEmpty(BookName))
                {
                    var temp = _bookRepository.Get(b => b.Name == BookName).FirstOrDefault();
                    var variant = _Inventory.GetBookByID(temp.Book_Id);
                    ViewData["ISBN"] = variant.ISBN;
                    ViewData["Author"] = variant.Author;
                    ViewData["Summary"] = variant.Summary;
                }

                if (BookName != null)
                {
                    ViewData["Book"] = BookName;
                }
                ViewData["Types"] = _Inventory.GetTypes();
                ViewData["Publishers"] = _Inventory.GetAllPublishers();
                ViewData["Genres"] = _Inventory.GetGenres();
                ViewData["Conditions"] = _Inventory.GetConditions();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in loading dropdownlists for Books Action Method");
            }

            ViewData["check"] = Constants.ErrorStatusYes;

            return View();
        }

        [HttpPost] //Post Data from forms to tables
        public IActionResult EditBookDetails(BooksViewModel bookview)
        {
            _logger.LogInformation("Posting new book details from form to Database ");
            bookview.UpdatedBy = @User.Claims.FirstOrDefault(c => c.Type.Equals("cognito:username"))?.Value;
            bookview.UpdatedOn = DateTime.Now;
            bookview.Active = true;
            if (String.IsNullOrEmpty(bookview.Author))
            {
                var temp = _bookRepository.Get(b => b.Name == bookview.BookName).FirstOrDefault();
                var variant = _Inventory.GetBookByID(temp.Book_Id);
                bookview.Author = variant.Author;
                bookview.Summary = variant.Summary;
            }
            if (ModelState.IsValid)
            {
                BooksDto booksDto = _mapper.Map<BooksDto>(bookview);
                var status = _Inventory.AddToTables(booksDto);

                if (status != 1)
                {

                    ViewData["ErrorStatus"] = Constants.ErrorStatusYes;



                    ViewData["Types"] = _Inventory.GetTypes();
                    ViewData["Publishers"] = _Inventory.GetAllPublishers();
                    ViewData["Genres"] = _Inventory.GetGenres();
                    ViewData["Conditions"] = _Inventory.GetConditions();
                    return View(bookview);
                }

                var temp = _bookRepository.Get(b => b.Name == bookview.BookName).FirstOrDefault();
                var BookId = temp.Book_Id;
                return RedirectToAction("BookDetails", new { BookId });

            }

            return View(bookview);
        }


        [HttpGet]
        public IActionResult AddPublishers()
        {

            ViewData["Status"] = Constants.AddPublisherMessage;
            ViewData["Publishers"] = _Inventory.GetAllPublishers();


            return View();


        }

        [HttpPost]
        public IActionResult AddPublishers(Publisher publisher, string source)
        {
            try
            {
                var status = _Inventory.AddPublishers(publisher);

                if (status == 0)
                {
                    return RedirectToAction("EditBookDetails");
                }
                if (status == 1)
                {
                    ViewData["Status"] = Constants.PublisherExistsStatus;
                }
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
                var status = _Inventory.AddGenres(genre);

                if (status == 0)
                {
                    return RedirectToAction("EditBookDetails");
                }
                if (status == 1)
                {
                    ViewData["Status"] = Constants.GenreExistsStatus;
                }
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
                var status = _Inventory.AddBookTypes(booktype);

                if (status == 0)
                {
                    return RedirectToAction("EditBookDetails");
                }
                if (status == 1)
                {
                    ViewData["Status"] = Constants.TypeExistsStatus;
                }
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
                var status = _Inventory.AddBookConditions(bookcondition);

                if (status == 0)
                {
                    return RedirectToAction("EditBookDetails");
                }
                if (status == 1)
                {
                    ViewData["Status"] = Constants.ConditionExistsStatus;
                }
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
            ViewData["Publishers"] = _Inventory.GetAllPublishers();
            return View();
        }


        [HttpPost]
        public IActionResult EditPublisher(string Actual, string Name)
        {
            if (Actual == Name)
            {
                ViewData["Status"] = "You seem to have not made any change , Please Recheck";
            }

            else
            {
                _Inventory.EditPublisher(Actual, Name);
                ViewData["Status"] = "Successfully updated the existing records";

            }
            ViewData["Publishers"] = _Inventory.GetAllPublishers();

            return View();
        }

        [HttpGet]
        public IActionResult EditGenre()
        {
            ViewData["Genres"] = _Inventory.GetGenres();
            return View();
        }

        public IActionResult EditGenre(string Actual, string Name)
        {
            if (Actual == Name)
            {
                ViewData["Status"] = "Successfully updated the existing records";
            }

            else
            {
                _Inventory.EditGenre(Actual, Name);
                ViewData["Status"] = "Successfully updated the existing records";

            }
            ViewData["Genres"] = _Inventory.GetGenres();

            return View();
        }

        [HttpGet]
        public IActionResult EditCondition()
        {
            ViewData["Conditions"] = _Inventory.GetConditions();
            return View();
        }

        public IActionResult EditCondition(string Actual, string Name)
        {
            if (Actual == Name)
            {
                ViewData["Status"] = "Successfully updated the existing records";
            }

            else
            {
                _Inventory.EditCondition(Actual, Name);
                ViewData["Status"] = "Successfully updated the existing records";

            }
            ViewData["Conditions"] = _Inventory.GetConditions();

            return View();
        }

        [HttpGet]
        public IActionResult EditType()
        {
            ViewData["Types"] = _Inventory.GetTypes();
            return View();
        }

        public IActionResult EditType(string Actual, string Name)
        {
            if (Actual == Name)
            {
                ViewData["Status"] = "Successfully updated the existing records";
            }

            else
            {
                _Inventory.EditType(Actual, Name);
                ViewData["Status"] = "Successfully updated the existing records";

            }
            ViewData["Types"] = _Inventory.GetTypes();

            return View();
        }


        [HttpGet]
        public IActionResult BookDetails(long BookId)
        {
            _logger.LogInformation("Loading Book Details on Click in search page");

            FetchBooksViewModel books = new FetchBooksViewModel();
            try
            {
                var bookdetails = _Inventory.GetBookByID(BookId);
                IEnumerable<BookDetailsViewModel> bookDetails = _mapper.Map<IEnumerable<BookDetailsDto>, IEnumerable<BookDetailsViewModel>>(_Inventory.GetDetails(BookId));
                books.publisher = bookdetails.Publisher.Name;
                books.genre = bookdetails.Genre.Name;
                books.BookType = bookdetails.BookType.TypeName;
                books.BookName = bookdetails.BookName;
                books.Books = bookDetails;
                books.front_url = bookdetails.front_url;
                books.back_url = bookdetails.back_url;
                books.left_url = bookdetails.left_url;
                books.right_url = bookdetails.right_url;
                books.Author = bookdetails.Author;
                books.ISBN = bookdetails.ISBN;
                books.Summary = bookdetails.Summary;

                ViewData["Types"] = _Inventory.GetFormatsOfTheSelectedBook(bookdetails.BookName);
                ViewData["Conditions"] = _Inventory.GetConditionsOfTheSelectedBook(bookdetails.BookName);
                ViewData["status"] = Constants.BookDetailsStatusDetails;

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in displaying Book Details");
            }
            return View(books);



        }

        [HttpPost]
        public IActionResult BookDetails(string type, string condition_chosen, string BookName, FetchBooksViewModel book)
        {
            /*
                *  function to add details to the Book table
                
            if (String.IsNullOrEmpty(type))
            {
                type = _context.Book.Include(b => b.Type)
                        .Where(b => b.Name == BookName).ToList()[0].Type.TypeName;               
            }
            */
            _logger.LogInformation("Loading books to be displayed in table based on type and condition chosen");
            try
            {
                ViewData["Types"] = _Inventory.GetFormatsOfTheSelectedBook(BookName);
                ViewData["Conditions"] = _Inventory.GetConditionsOfTheSelectedBook(BookName);
                ViewData["status"] = "List";

                IEnumerable<BookDetailsViewModel> bookDetails = _mapper.Map<List<BookDetailsDto>, IEnumerable<BookDetailsViewModel>>(_Inventory.GetRelevantBooks(BookName, type, condition_chosen));
                ViewData["Books"] = bookDetails;
                var lis = _Inventory.GetRelevantBooks(BookName, type, condition_chosen);
                if (lis.Count == 0)
                {
                    var temp = _bookRepository.Get(b => b.Name == BookName).FirstOrDefault();
                    var variant = _Inventory.GetBookByID(temp.Book_Id);
                    book.genre = variant.Genre.Name;
                    book.Author = variant.Author;
                    book.Summary = variant.Summary;
                    ViewData["status"] = Constants.BookDetailsStatusDetails;
                    ViewData["fetchstatus"] = Constants.CombinationErrorStatus;
                    return View(book);
                }
                book.BookType = lis[0].BookType.TypeName;
                book.publisher = lis[0].Publisher.Name;
                book.genre = lis[0].Genre.Name;
                book.front_url = lis[0].front_url;
                book.back_url = lis[0].back_url;
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

        public IActionResult SearchBeta(string searchfilter, string searchby, int pageNum, string ViewStyle, string SortBy, string ascdesc, string pagination)
        {
            _logger.LogInformation("Search Page");
            try
            {
                var stats = _Inventory.DashBoard();
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

                if ((String.IsNullOrEmpty(searchby) || String.IsNullOrEmpty(searchfilter)))
                {


                    var books = _Inventory.GetAllBooks(pageNum, ViewStyle, SortBy, ascdesc);
                    books.SortBy = SortBy;
                    
                    SearchBookViewModel viewModel = _mapper.Map<SearchBookViewModel>(books);

                    return View(viewModel);
                }

                else
                {

                    var books = _Inventory.SearchBooks(searchby, searchfilter, ViewStyle, SortBy, pageNum, ascdesc);
                    books.SortBy = SortBy;

                    SearchBookViewModel viewModel = _mapper.Map<SearchBookViewModel>(books);

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
        public IActionResult UpdateDetails(int BookId, string Condition)
        {
            _logger.LogInformation("Load Edit Book Details page with pre-filled values");
            try
            {
                var details = _Inventory.UpdateDetails(BookId, Condition);

                return View(details);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in fetching prefilled values for edit book details page");
            }

            return View();
        }

        public IActionResult Submitchanges(BookDetailsDto details)
        {
            _logger.LogInformation("Posting the Edit Book form values to database");
            try
            {
                details.UpdatedBy = @User.Claims.FirstOrDefault(c => c.Type.Equals("cognito:username"))?.Value;
                details.UpdatedOn = DateTime.Now;
                _Inventory.PushDetails(details);

            }

            catch (Exception e)
            {
                _logger.LogError(e, "Error in posting Edited Book details to database");
            }
            return RedirectToAction("SearchBeta");
        }

        public IActionResult Dashboard(FetchBooksViewModel Book)
        {
            _logger.LogInformation("Dashboard Display");

            _emailSender.SendInventoryLowEmail(_Inventory.ScreenInventory(), Constants.BoBsEmailAddress);
            try
            {
                var stats = _Inventory.DashBoard();
                ViewData["ordes_top_genre"] = stats[0].First().Key;
                ViewData["ordes_top_genre_count"] = stats[0].First().Value;

                ViewData["ordes_top_type"] = stats[1].First().Key;
                ViewData["ordes_top_type_count"] = stats[1].First().Value;

                ViewData["ordes_top_publisher"] = stats[2].First().Key;
                ViewData["ordes_top_publisher_count"] = stats[2].First().Value;

                ViewData["ordes_top_name"] = stats[3].First().Key;
                ViewData["ordes_top_name_count"] = stats[3].First().Value;

                ViewData["orders_genre"] = stats[0];
                ViewData["orders_type"] = stats[1];
                ViewData["orders_publisher"] = stats[2];
                ViewData["orders_name"] = stats[3];


                var a = stats[4];

                List<int> InventoryStats = new List<int>();
                foreach (var i in a)
                    InventoryStats.Add(i.Value);
                ViewData["Inventory"] = InventoryStats;

                var b = stats[5];
                List<int> OrdersStats = new List<int>();
                foreach (var i in b)
                    OrdersStats.Add(i.Value);
                ViewData["Orders"] = OrdersStats;
            }

            catch (Exception e)
            {
                _logger.LogError("Error in displaying dasboard statistics", e);
                return RedirectToAction("Error", "Home");
            }
            return View();
        }


        [HttpGet]
        public IActionResult AutoSuggest(string searchby)
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = _Inventory.autosuggest(term);

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
