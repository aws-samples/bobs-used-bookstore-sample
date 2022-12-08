using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Services;
using Bookstore.Admin.ViewModel.Inventory;
using Bookstore.Admin.Mappers.Inventory;
using Bookstore.Services.Filters;
using Microsoft.AspNetCore.Routing;
using System.Linq;

namespace AdminSite.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly IInventoryService inventoryService;
        private readonly IReferenceDataService referenceDataService;

        public InventoryController(IInventoryService inventoryService, IReferenceDataService referenceDataService)
        {
            this.inventoryService = inventoryService;
            this.referenceDataService = referenceDataService;
        }

        public IActionResult Index(InventoryFilters filters, int pageIndex = 1, int pageSize = 10)
        {
            var books = inventoryService.GetBooks(filters, pageIndex, pageSize);
            var referenceData = referenceDataService.GetReferenceData();
            var model = books.ToInventoryIndexViewModel();

            model = model.PopulateReferenceData(referenceData);
            model.RouteData = new RouteValueDictionary(filters).ToDictionary(x => $"{nameof(filters)}.{x.Key}", x => x.Value?.ToString());

            return View(model);
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
    }
}