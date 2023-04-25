using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Web.Areas.Admin.Models.Inventory;
using Bookstore.Domain.Books;
using Bookstore.Domain.ReferenceData;

namespace Bookstore.Web.Areas.Admin.Controllers
{
    public class InventoryController : AdminAreaControllerBase
    {
        private readonly IBookService bookService;
        private readonly IReferenceDataService referenceDataService;

        public InventoryController(IBookService bookService, IReferenceDataService referenceDataService)
        {
            this.bookService = bookService;
            this.referenceDataService = referenceDataService;
        }

        public async Task<IActionResult> Index(BookFilters filters, int pageIndex = 1, int pageSize = 10)
        {
            var books = await bookService.GetBooksAsync(filters, pageIndex, pageSize);
            var referenceDataItems = await referenceDataService.GetAllReferenceDataAsync();

            return View(new InventoryIndexViewModel(books, referenceDataItems));
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await bookService.GetBookAsync(id);

            return View(new InventoryDetailsViewModel(book));
        }

        public async Task<IActionResult> Create()
        {
            var referenceDataItemDtos = await referenceDataService.GetAllReferenceDataAsync();

            return View("CreateUpdate", new InventoryCreateUpdateViewModel(referenceDataItemDtos));
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var referenceDataItemDtos = await referenceDataService.GetAllReferenceDataAsync();

                model.AddReferenceData(referenceDataItemDtos);

                return View("CreateUpdate", model);
            }

            var dto = new CreateBookDto(
                model.Name, 
                model.Author, 
                model.SelectedBookTypeId, 
                model.SelectedConditionId, 
                model.SelectedGenreId, 
                model.SelectedPublisherId, 
                model.Year, 
                model.ISBN, 
                model.Summary, 
                model.Price, 
                model.Quantity, 
                model.CoverImage?.OpenReadStream(), 
                model.CoverImage?.FileName);

            await bookService.AddAsync(dto);

            TempData["Message"] = $"{model.Name} has been added to inventory";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var book = await bookService.GetBookAsync(id);
            var referenceDataDtos = await referenceDataService.GetAllReferenceDataAsync();

            return View("CreateUpdate", new InventoryCreateUpdateViewModel(referenceDataDtos, book));
        }

        [HttpPost]
        public async Task<IActionResult> Update(InventoryCreateUpdateViewModel model)
        {
            var dto = new UpdateBookDto(
                model.Id,
                model.Name,
                model.Author,
                model.SelectedBookTypeId,
                model.SelectedConditionId,
                model.SelectedGenreId,
                model.SelectedPublisherId,
                model.Year,
                model.ISBN,
                model.Summary,
                model.Price,
                model.Quantity,
                model.CoverImage?.OpenReadStream(),
                model.CoverImage?.FileName);

            await bookService.UpdateAsync(dto);

            TempData["Message"] = $"{model.Name} has been updated";

            return RedirectToAction("Index");
        }
    }
}