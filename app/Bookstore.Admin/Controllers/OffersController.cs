using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bookstore.Domain.Offers;
using Bookstore.Services;
using Bookstore.Admin.Mappers.Offers;

namespace Bookstore.Admin.Controllers
{

    [Authorize]
    public class OffersController : Controller
    {
        private readonly IOfferService offerService;

        public OffersController(IOfferService offerService)
        {
            this.offerService = offerService;
        }

        public IActionResult Index(int pageIndex = 1, int pageSize = 10)
        {
            var offers = offerService.GetOffers(User.Identity.Name, pageIndex, pageSize);

            return View(offers.ToOfferIndexViewModel());
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