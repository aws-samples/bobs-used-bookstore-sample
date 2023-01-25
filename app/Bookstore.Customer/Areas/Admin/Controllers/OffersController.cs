using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bookstore.Domain.Offers;
using Bookstore.Services;
using Bookstore.Services.Filters;
using Services;
using Bookstore.Web.Areas.Admin.Mappers.Inventory;
using Bookstore.Web.Areas.Admin.Mappers.Offers;

namespace Bookstore.Web.Areas.Admin.Controllers
{
    public class OffersController : AdminAreaControllerBase
    {
        private readonly IOfferService offerService;
        private readonly IReferenceDataService referenceDataService;

        public OffersController(IOfferService offerService, IReferenceDataService referenceDataService)
        {
            this.offerService = offerService;
            this.referenceDataService = referenceDataService;
        }

        public IActionResult Index(OfferFilters filters, int pageIndex = 1, int pageSize = 10)
        {
            var offers = offerService.GetOffers(filters, pageIndex, pageSize);
            var referenceData = referenceDataService.GetReferenceData();
            var model = offers.ToOfferIndexViewModel();

            model.PopulateReferenceData(referenceData);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveAsync(int id)
        {
            return await UpdateOfferStatus(id, OfferStatus.Approved, "The offer has been approved");
        }

        [HttpPost]
        public async Task<IActionResult> RejectAsync(int id)
        {
            return await UpdateOfferStatus(id, OfferStatus.Rejected, "The offer has been rejected");
        }

        [HttpPost]
        public async Task<IActionResult> ReceivedAsync(int id)
        {
            return await UpdateOfferStatus(id, OfferStatus.Received, "The book has been received");
        }

        [HttpPost]
        public async Task<IActionResult> PaidAsync(int id)
        {
            return await UpdateOfferStatus(id, OfferStatus.Paid, "The customer has been paid");
        }

        private async Task<IActionResult> UpdateOfferStatus(int id, OfferStatus status, string message)
        {
            var offer = offerService.GetOffer(id);

            offer.OfferStatus = status;

            await offerService.SaveOfferAsync(offer, User.Identity.Name);

            TempData["Message"] = message;

            return RedirectToAction("Index");
        }
    }
}