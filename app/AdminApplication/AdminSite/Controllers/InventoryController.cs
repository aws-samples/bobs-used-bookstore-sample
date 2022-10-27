using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DataAccess.Dtos;
using DataAccess.Repository.Interface;
using DataAccess.Repository.Interface.InventoryInterface;
using DataModels.Books;
using AdminSite.ViewModel;
using AdminSite.ViewModel.ManageInventory;
using AdminSite.ViewModel.SearchBooks;
using Type = DataModels.Books.Type;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminSite.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private const int NumberOfDetails = 5;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IInventory _inventory;
        private readonly ILogger<InventoryController> _logger;
        private readonly IMapper _mapper;
        private string updateSuccess = Resource.ResourceManager.GetString("UpdateSuccessMessage");


        public InventoryController(IGenericRepository<Book> bookRepository,
                                   IMapper mapper,
                                   IInventory inventory,
                                   ILogger<InventoryController> logger)
        {
            _inventory = inventory;
            _logger = logger;
            _mapper = mapper;
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public IActionResult EditBookDetails(string bookName)
        {
            _logger.LogInformation("Loading : Add New Book View");

            var model = new BooksViewModel
            {
                Publishers = _inventory.GetAllPublishers().Select(x => new SelectListItem(x.Name, x.Name)),
                Genres = _inventory.GetGenres().Select(x => new SelectListItem(x.Name, x.Name)),
                BookTypes = _inventory.GetTypes().Select(x => new SelectListItem(x.TypeName, x.TypeName)),
                BookConditions = _inventory.GetConditions().Select(x => new SelectListItem(x.ConditionName, x.ConditionName))
            };

            if (!string.IsNullOrEmpty(bookName))
            {
                var temp = _bookRepository.Get(b => b.Name == bookName).FirstOrDefault();
                var variant = _inventory.GetBookByID(temp.Book_Id);

                model.BookName = variant.BookName;
                model.ISBN = variant.ISBN;
                model.Author = variant.Author;
                model.Summary = variant.Summary;
            }

            ViewData["check"] = Constants.ErrorStatusYes;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditBookDetails(BooksViewModel bookview)
        {
            _logger.LogInformation("Posting new book details from form to Database ");

            bookview.UpdatedBy = User.Identity.Name;
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
                var status = await _inventory.AddToTablesAsync(booksDto);

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
            ViewData["Status"] = Resource.ResourceManager.GetString("AddPublisherMessage");
            ViewData["Publishers"] = _inventory.GetAllPublishers();

            return View();
        }

        [HttpPost]
        public IActionResult AddPublishers(Publisher publisher)
        {
            try
            {
                var status = _inventory.AddPublishers(publisher);

                if (status)
                    return RedirectToAction("EditBookDetails");
                ViewData["Status"] = Resource.ResourceManager.GetString("PublisherExistsStatus");
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
            ViewData["Status"] = Resource.ResourceManager.GetString("AddGenreMessage");
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
                ViewData["Status"] = Resource.ResourceManager.GetString("GenreExistsStatus");
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
            ViewData["Status"] = Resource.ResourceManager.GetString("AddTypeMessage");
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
                ViewData["Status"] = Resource.ResourceManager.GetString("TypeExistsStatus");
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
            ViewData["Status"] = Resource.ResourceManager.GetString("AddConditionsMessage");
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
                ViewData["Status"] = Resource.ResourceManager.GetString("ConditionExistsStatus");
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
                ViewData["Status"] = updateSuccess;
            }
            else
            {
                _inventory.EditCondition(actual, name);
                ViewData["Status"] = updateSuccess;
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
                ViewData["Status"] = updateSuccess;
            }
            else
            {
                _inventory.EditType(actual, name);
                ViewData["Status"] = updateSuccess;
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
                        _inventory.GetRelevantBooks(bookName, type, book.ConditionChosen));
                ViewData["Books"] = bookDetails;
                var lis = _inventory.GetRelevantBooks(bookName, type, book.ConditionChosen);
                if (lis.Count == 0)
                {
                    var temp = _bookRepository.Get(b => b.Name == bookName).FirstOrDefault();
                    var variant = _inventory.GetBookByID(temp.Book_Id);
                    book.Genre = variant.Genre.Name;
                    book.Author = variant.Author;
                    book.Summary = variant.Summary;
                    ViewData["status"] = Constants.BookDetailsStatusDetails;
                    ViewData["fetchstatus"] = Resource.ResourceManager.GetString("CombinationErrorStatus");
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
                details.UpdatedBy = User.Identity.Name;
                details.UpdatedOn = DateTime.Now.ToUniversalTime();
                await _inventory.PushDetailsAsync(details);
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
                var names = _inventory.AutoSuggest(term);

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
