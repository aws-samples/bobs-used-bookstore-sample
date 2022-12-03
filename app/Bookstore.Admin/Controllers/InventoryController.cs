using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Services;
using Bookstore.Admin.ViewModel.Inventory;
using Bookstore.Admin.Mappers.Inventory;

namespace AdminSite.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly ILogger<InventoryController> logger;
        private readonly IInventoryService inventoryService;
        private readonly IReferenceDataService referenceDataService;

        public InventoryController(ILogger<InventoryController> logger,
                                   IInventoryService inventoryService,
                                   IReferenceDataService referenceDataService)
        {
            this.logger = logger;
            this.inventoryService = inventoryService;
            this.referenceDataService = referenceDataService;
        }

        public IActionResult Index(int pageIndex = 1, int pageSize = 10)
        {
            var books = inventoryService.GetBooks(User.Identity.Name, pageIndex, pageSize);

            return View(books.ToInventoryIndexViewModel());
        }

        public IActionResult Details(int id)
        {
            var book = inventoryService.GetBook(id);

            return View(book.ToInventoryDetailsViewModel());
        }

        public IActionResult Create()
        {
            var model = new InventoryCreateUpdateViewModel();
            var referenceData = referenceDataService.GetReferenceData();

            model = model.PopulateReferenceData(referenceData);

            return View("CreateUpdate", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View("CreateUpdate", model);

            var book = model.ToBook();

            await inventoryService.SaveAsync(
                book, 
                model.FrontImage,
                model.BackImage,
                model.LeftImage,
                model.RightImage,
                User.Identity.Name);

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            var book = inventoryService.GetBook(id);
            var referenceData = referenceDataService.GetReferenceData();
            var model = book.ToInventoryCreateUpdateViewModel();

            model = model.PopulateReferenceData(referenceData);

            return View("CreateUpdate", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(InventoryCreateUpdateViewModel model)
        {
            var book = inventoryService.GetBook(model.Id);

            model.ToBook(book);

            await inventoryService.SaveAsync(
                book, 
                model.FrontImage, 
                model.BackImage,
                model.LeftImage,
                model.RightImage,
                User.Identity.Name);

            return RedirectToAction("Index");
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

        //public IActionResult Dashboard(FetchBooksViewModel book)
        //{
        //    logger.LogInformation("Dashboard Display");

        //    // _emailSender.SendInventoryLowEmail(_Inventory.ScreenInventory(), Constants.BoBsEmailAddress);
        //    try
        //    {
        //        //var stats = _inventory.DashBoard(NumberOfDetails);

        //        //if (stats[0].Count() != 0)
        //        //{
        //        //    ViewData["orders_top_genre"] = stats[0].First().Key;
        //        //    ViewData["orders_top_genre_count"] = stats[0].First().Value;
        //        //}

        //        //if (stats[1].Count() != 0)
        //        //{
        //        //    ViewData["orders_top_type"] = stats[1].First().Key;
        //        //    ViewData["orders_top_type_count"] = stats[1].First().Value;
        //        //}

        //        //if (stats[2].Count != 0)
        //        //{
        //        //    ViewData["orders_top_publisher"] = stats[2].First().Key;
        //        //    ViewData["orders_top_publisher_count"] = stats[2].First().Value;
        //        //}

        //        //if (stats[1].Count() != 0)
        //        //{
        //        //    ViewData["orders_top_name"] = stats[3].First().Key;
        //        //    ViewData["orders_top_name_count"] = stats[3].First().Value;
        //        //}

        //        //ViewData["orders_genre"] = stats[0];
        //        //ViewData["orders_type"] = stats[1];
        //        //ViewData["orders_publisher"] = stats[2];
        //        //ViewData["orders_name"] = stats[3];

        //        //var a = stats[4];

        //        //var inventoryStats = new List<int>();
        //        //foreach (var i in a) inventoryStats.Add(i.Value);
        //        //ViewData["Inventory"] = inventoryStats;

        //        //var b = stats[5];
        //        //var ordersStats = new List<int>();
        //        //foreach (var i in b) ordersStats.Add(i.Value);
        //        //ViewData["Orders"] = ordersStats;
        //    }
        //    catch (Exception e)
        //    {
        //        logger.LogError("Error in displaying dashboard statistics", e);
        //        return RedirectToAction("Error", "Home");
        //    }

        //    return View();
        //}
    }
}
