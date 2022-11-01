using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using AdminSite.ViewModel;
using BookType = Bookstore.Domain.Books.BookType;
using Microsoft.AspNetCore.Authorization;
using Services;
using AdminSite.ViewModel.Inventory;
using Bookstore.Domain.Books;
using Bookstore.Admin;
using Bookstore.Data.Repository.Interface;
using Amazon.SimpleSystemsManagement.Model;
using Bookstore.Admin.ViewModel.Inventory;

namespace AdminSite.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly ILogger<InventoryController> logger;
        private readonly IMapper mapper;
        private readonly IInventoryService inventoryService;
        private readonly IReferenceDataService referenceDataService;

        public InventoryController(ILogger<InventoryController> logger,
                                   IMapper mapper,
                                   IInventoryService inventoryService,
                                   IReferenceDataService referenceDataService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.inventoryService = inventoryService;
            this.referenceDataService = referenceDataService;
        }

        [HttpGet]
        public IActionResult Index(int startIndex = 0, int count = 10)
        {
            var books = inventoryService.GetBooks(User.Identity.Name, startIndex, count);

            var viewModel = mapper.Map<InventoryIndexViewModel>(books);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            logger.LogInformation("Loading Book Details on Click in search page");

            var book = inventoryService.GetBook(id);
            var viewModel = mapper.Map<InventoryDetailsViewModel>(book);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var referenceData = referenceDataService.GetAllReferenceData();

            var viewModel = mapper.Map<InventoryCreateUpdateViewModel>(referenceData);

            return View("CreateUpdate", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View("CreateUpdate", model);

            //var book = _mapper.Map<Book>(model);

            //_database.Book.Add(book);

            //await _database.SaveChangesAsync();

            return RedirectToAction("Index", "Inventory");
        }

        [HttpGet]
        public IActionResult EditBookDetails(int? bookId = null)
        {
            logger.LogInformation("Loading : Add New Book View");

            var model = new InventoryCreateUpdateViewModel();

            if (bookId.HasValue)
            {
                //var book = _inventory.GetBookByID(bookId.Value);
                var book = inventoryService.GetBook(bookId.Value);

                //TODO handle book not found

                model = mapper.Map<InventoryCreateUpdateViewModel>(book);
            }

            //model.Publishers = _inventory.GetAllPublishers().Select(x => new SelectListItem(x.Name, x.Name));
            //model.Genres = _inventory.GetGenres().Select(x => new SelectListItem(x.Name, x.Name));
            //model.BookTypes = _inventory.GetTypes().Select(x => new SelectListItem(x.TypeName, x.TypeName));
            //model.BookConditions = _inventory.GetConditions().Select(x => new SelectListItem(x.ConditionName, x.ConditionName));

            ViewData["check"] = Constants.ErrorStatusYes;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditBookDetails(InventoryCreateUpdateViewModel bookview)
        {
            logger.LogInformation("Posting new book details from form to Database ");

            bookview.UpdatedBy = User.Identity.Name;
            bookview.UpdatedOn = DateTime.Now.ToUniversalTime();
            bookview.Active = true;

            var book = mapper.Map<Book>(bookview);

            await inventoryService.SaveBookAsync(book);

            //if (string.IsNullOrEmpty(bookview.Author))
            //{
            //    var temp = _bookRepository.Get(b => b.Name == bookview.BookName).FirstOrDefault();
            //    var variant = _inventory.GetBookByID(temp.Book_Id);
            //    bookview.Author = variant.Author;
            //    bookview.Summary = variant.Summary;
            //}

            //if (ModelState.IsValid)
            //{
            //    var booksDto = _mapper.Map<BooksDto>(bookview);
            //    var status = await _inventory.AddToTablesAsync(booksDto);

            //    if (!status)
            //    {
            //        ViewData["ErrorStatus"] = Constants.ErrorStatusYes;

            //        ViewData["Types"] = _inventory.GetTypes();
            //        ViewData["Publishers"] = _inventory.GetAllPublishers();
            //        ViewData["Genres"] = _inventory.GetGenres();
            //        ViewData["Conditions"] = _inventory.GetConditions();
            //        return View(bookview);
            //    }

            //    var temp = _bookRepository.Get(b => b.Name == bookview.BookName).FirstOrDefault();
            //    var bookId = temp.Book_Id;
            //    return RedirectToAction("BookDetails", new { BookId = bookId });
            //}

            return View(bookview);
        }

        [HttpGet]
        public IActionResult AddPublishers()
        {
            ViewData["Status"] = Resource.ResourceManager.GetString("AddPublisherMessage");
            //ViewData["Publishers"] = _inventory.GetAllPublishers();

            return View();
        }

        [HttpPost]
        public IActionResult AddPublishers(Publisher publisher)
        {
            //try
            //{
            //    var status = _inventory.AddPublishers(publisher);

            //    if (status)
            //        return RedirectToAction("EditBookDetails");
            //    ViewData["Status"] = Resource.ResourceManager.GetString("PublisherExistsStatus");
            //}

            //catch (Exception e)
            //{
            //    _logger.LogError(e, "Error in adding a new publisher");
            //}

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
            //try
            //{
            //    var status = _inventory.AddGenres(genre);

            //    if (status)
            //        return RedirectToAction("EditBookDetails");
            //    ViewData["Status"] = Resource.ResourceManager.GetString("GenreExistsStatus");
            //}

            //catch (Exception e)
            //{
            //    _logger.LogError(e, "Error in adding a new genre");
            //}

            return View();
        }

        [HttpGet]
        public IActionResult AddBookTypes()
        {
            ViewData["Status"] = Resource.ResourceManager.GetString("AddTypeMessage");
            return View();
        }

        [HttpPost]
        public IActionResult AddBookTypes(BookType booktype)
        {
            //try
            //{
            //    var status = _inventory.AddBookTypes(booktype);

            //    if (status)
            //        return RedirectToAction("EditBookDetails");
            //    ViewData["Status"] = Resource.ResourceManager.GetString("TypeExistsStatus");
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e, "Error in adding a new type");
            //}

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
            //try
            //{
            //    var status = _inventory.AddBookConditions(bookcondition);

            //    if (status)
            //        return RedirectToAction("EditBookDetails");
            //    ViewData["Status"] = Resource.ResourceManager.GetString("ConditionExistsStatus");
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e, "Error in adding a new condition");
            //}

            return View();
        }

        [HttpGet]
        public IActionResult EditPublisher()
        {
            //ViewData["Publishers"] = _inventory.GetAllPublishers();
            return View();
        }

        [HttpPost]
        public IActionResult EditPublisher(string actual, string name)
        {
            //if (string.CompareOrdinal(actual, name) == 0)
            //{
            //    ViewData["Status"] = "You seem to have not made any change , Please Recheck";
            //}
            //else
            //{
            //    _inventory.EditPublisher(actual, name);
            //    ViewData["Status"] = "Successfully updated the existing records";
            //}

            //ViewData["Publishers"] = _inventory.GetAllPublishers();

            return View();
        }

        [HttpGet]
        public IActionResult EditGenre()
        {
            //ViewData["Genres"] = _inventory.GetGenres();
            return View();
        }

        public IActionResult EditGenre(string actual, string name)
        {
            //if (string.CompareOrdinal(actual, name) == 0)
            //{
            //    ViewData["Status"] = "Successfully updated the existing records";
            //}
            //else
            //{
            //    _inventory.EditGenre(actual, name);
            //    ViewData["Status"] = "Successfully updated the existing records";
            //}

            //ViewData["Genres"] = _inventory.GetGenres();

            return View();
        }

        [HttpGet]
        public IActionResult EditCondition()
        {
            //ViewData["Conditions"] = _inventory.GetConditions();
            return View();
        }

        public IActionResult EditCondition(string actual, string name)
        {
            //if (string.CompareOrdinal(actual, name) == 0)
            //{
            //    //ViewData["Status"] = updateSuccess;
            //}
            //else
            //{
            //    _inventory.EditCondition(actual, name);
            //    //ViewData["Status"] = updateSuccess;
            //}

            //ViewData["Conditions"] = _inventory.GetConditions();

            return View();
        }

        [HttpGet]
        public IActionResult EditType()
        {
            //ViewData["Types"] = _inventory.GetTypes();
            return View();
        }

        public IActionResult EditType(string actual, string name)
        {
            //if (string.CompareOrdinal(actual, name) == 0)
            //{
            //    //ViewData["Status"] = updateSuccess;
            //}
            //else
            //{
            //    _inventory.EditType(actual, name);
            //    //ViewData["Status"] = updateSuccess;
            //}

            //ViewData["Types"] = _inventory.GetTypes();

            return View();
        }

        public IActionResult SearchBeta(string searchfilter, string searchby, int pageNum, string viewStyle,
            string sortBy, string ascdesc, string pagination)
        {
            logger.LogInformation("Search Page");

            //try
            //{
            //    var stats = _inventory.DashBoard(NumberOfDetails);
            //    ViewData["genre"] = stats[0].OrderByDescending(x => x.Value).First().Key;
            //    ViewData["type"] = stats[1].OrderByDescending(x => x.Value).First().Key;
            //    ViewData["publisher"] = stats[2].OrderByDescending(x => x.Value).First().Key;
            //    ViewData["name"] = stats[3].OrderByDescending(x => x.Value).First().Key;
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e, "Error in loading search page dashboard");
            //}

            //try
            //{
            //    if (pageNum == 0) pageNum++;

            //    if (string.IsNullOrEmpty(searchby) || string.IsNullOrEmpty(searchfilter))
            //    {
            //        var books = _inventory.GetAllBooks(pageNum, viewStyle, sortBy, ascdesc);
            //        books.SortBy = sortBy;

            //        var viewModel = _mapper.Map<SearchBookViewModel>(books);

            //        return View(viewModel);
            //    }
            //    else
            //    {
            //        var books = _inventory.SearchBooks(searchby, searchfilter, viewStyle, sortBy, pageNum, ascdesc);
            //        books.SortBy = sortBy;

            //        var viewModel = _mapper.Map<SearchBookViewModel>(books);

            //        return View(viewModel);
            //    }
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e, "Error in loading search page");
            //}

            return View();
        }

        [HttpPost]
        public IActionResult UpdateDetails(int bookId, string condition)
        {
            logger.LogInformation("Load Edit Book Details page with pre-filled values");
            //try
            //{
            //    var details = _inventory.UpdateDetails(bookId, condition);

            //    var viewModel = _mapper.Map<BookDetailsViewModel>(details);

            //    return View(viewModel);
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e, "Error in fetching prefilled values for edit book details page");
            //}

            return View();
        }

        //public async Task<IActionResult> SubmitChangesAsync(BookDetailsDto details)
        //{
        //    _logger.LogInformation("Posting the Edit Book form values to database");
        //    try
        //    {
        //        details.UpdatedBy = User.Identity.Name;
        //        details.UpdatedOn = DateTime.Now.ToUniversalTime();
        //        await _inventory.PushDetailsAsync(details);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e, "Error in posting Edited Book details to database");
        //    }

        //    return RedirectToAction("SearchBeta");
        //}

        public IActionResult Dashboard(FetchBooksViewModel book)
        {
            logger.LogInformation("Dashboard Display");

            // _emailSender.SendInventoryLowEmail(_Inventory.ScreenInventory(), Constants.BoBsEmailAddress);
            try
            {
                //var stats = _inventory.DashBoard(NumberOfDetails);

                //if (stats[0].Count() != 0)
                //{
                //    ViewData["orders_top_genre"] = stats[0].First().Key;
                //    ViewData["orders_top_genre_count"] = stats[0].First().Value;
                //}

                //if (stats[1].Count() != 0)
                //{
                //    ViewData["orders_top_type"] = stats[1].First().Key;
                //    ViewData["orders_top_type_count"] = stats[1].First().Value;
                //}

                //if (stats[2].Count != 0)
                //{
                //    ViewData["orders_top_publisher"] = stats[2].First().Key;
                //    ViewData["orders_top_publisher_count"] = stats[2].First().Value;
                //}

                //if (stats[1].Count() != 0)
                //{
                //    ViewData["orders_top_name"] = stats[3].First().Key;
                //    ViewData["orders_top_name_count"] = stats[3].First().Value;
                //}

                //ViewData["orders_genre"] = stats[0];
                //ViewData["orders_type"] = stats[1];
                //ViewData["orders_publisher"] = stats[2];
                //ViewData["orders_name"] = stats[3];

                //var a = stats[4];

                //var inventoryStats = new List<int>();
                //foreach (var i in a) inventoryStats.Add(i.Value);
                //ViewData["Inventory"] = inventoryStats;

                //var b = stats[5];
                //var ordersStats = new List<int>();
                //foreach (var i in b) ordersStats.Add(i.Value);
                //ViewData["Orders"] = ordersStats;
            }
            catch (Exception e)
            {
                logger.LogError("Error in displaying dashboard statistics", e);
                return RedirectToAction("Error", "Home");
            }

            return View();
        }

        //[HttpGet]
        //public IActionResult AutoSuggest(string searchby)
        //{
        //    try
        //    {
        //        var term = HttpContext.Request.Query["term"].ToString();
        //        var names = _inventory.AutoSuggest(term);

        //        return Ok(names);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e, "Error in loading autosearch results");
        //        return BadRequest();
        //    }
        //}
    }
}
