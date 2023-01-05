using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Bookstore.Customer;
using Bookstore.Services;
using Bookstore.Customer.Mappers;
using Microsoft.AspNetCore.Authorization;
using Bookstore.Customer.ViewModel.Resale;

namespace CustomerSite.Controllers
{
    public class ResaleController : Controller
    {
        private readonly IReferenceDataService referenceDataService;
        private readonly IOfferService offerService;

        public ResaleController(IReferenceDataService referenceDataService, IOfferService offerService)
        {
            this.referenceDataService = referenceDataService;
            this.offerService = offerService;
        }

        public IActionResult Index()
        {
            var offers = offerService.GetOffers(User.GetSub());

            return View(offers.ToResaleIndexViewModel());
        }

        public IActionResult Create()
        {
            var viewModel = new ResaleCreateViewModel();
            var referenceData = referenceDataService.GetReferenceData();

            viewModel.PopulateReferenceData(referenceData);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ResaleCreateViewModel resaleViewModel)
        {
            if (!ModelState.IsValid) return View();

            var offer = resaleViewModel.ToOffer();

            await offerService.CreateOfferAsync(offer, User.GetSub());

            return RedirectToAction(nameof(Index));
        }
    }
}