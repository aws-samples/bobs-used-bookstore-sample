using Bookstore.Domain.ReferenceData;
using Bookstore.Web.Areas.Admin.Models.ReferenceData;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookstore.Web.Areas.Admin.Controllers
{
    public class ReferenceDataController : AdminAreaControllerBase
    {
        private readonly IReferenceDataService referenceDataService;

        public ReferenceDataController(IReferenceDataService referenceDataService)
        {
            this.referenceDataService = referenceDataService;
        }

        public async Task<IActionResult> Index(ReferenceDataFilters filters, int pageIndex = 1, int pageSize = 10)
        {
            var referenceDataItems = await referenceDataService.GetReferenceDataAsync(filters, pageIndex, pageSize);

            return View(new ReferenceDataIndexViewModel(referenceDataItems, filters));
        }

        public IActionResult Create(ReferenceDataType? selectedReferenceDataType = null)
        {
            var model = new ReferenceDataItemCreateUpdateViewModel();

            if (selectedReferenceDataType.HasValue) model.SelectedReferenceDataType = selectedReferenceDataType.Value;

            return View("CreateUpdate", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReferenceDataItemCreateUpdateViewModel model)
        {
            var dto = new CreateReferenceDataItemDto(model.SelectedReferenceDataType, model.Text);

            await referenceDataService.CreateAsync(dto);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var referenceDataItem = await referenceDataService.GetReferenceDataItemAsync(id);

            return View("CreateUpdate", new ReferenceDataItemCreateUpdateViewModel(referenceDataItem));
        }

        [HttpPost]
        public async Task<IActionResult> Update(ReferenceDataItemCreateUpdateViewModel model)
        {
            var dto = new UpdateReferenceDataItemDto(model.Id, model.SelectedReferenceDataType, model.Text);

            await referenceDataService.UpdateAsync(dto);

            return RedirectToAction("Index");
        }
    }
}
