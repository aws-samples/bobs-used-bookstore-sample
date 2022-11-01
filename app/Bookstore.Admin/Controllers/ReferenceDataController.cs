using AutoMapper;
using Bookstore.Admin.ViewModel.ReferenceData;
using Bookstore.Domain.ReferenceData;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Bookstore.Admin.Controllers
{
    public class ReferenceDataController : Controller
    {
        private readonly IReferenceDataService referenceDataService;

        public ReferenceDataController(IReferenceDataService referenceDataService)
        {
            this.referenceDataService = referenceDataService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(ReferenceDataType? selectedReferenceDataType = null)
        {
            var model = new ReferenceDataCreateViewModel();

            if(selectedReferenceDataType.HasValue) model.SelectedReferenceDataType = selectedReferenceDataType.Value;

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ReferenceDataCreateViewModel model)
        {
            referenceDataService.Add(model.SelectedReferenceDataType, model.Text, User.Identity.Name);

            return RedirectToAction("Index", "Inventory");
        }
    }
}
