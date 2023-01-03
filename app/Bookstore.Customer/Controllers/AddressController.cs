using Bookstore.Customer;
using Bookstore.Customer.Mappers;
using Bookstore.Customer.ViewModel.Address;
using Bookstore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CustomerSite.Controllers
{
    public class AddressController : Controller
    {
        private readonly ICustomerService customerService;

        public AddressController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        public IActionResult Index()
        {
            var addresses = customerService.GetAddresses(User.GetSub());

            return View(addresses.ToAddressIndexViewModel());
        }

        public IActionResult Create(string returnUrl)
        {
            var model = new AddressCreateUpdateViewModel
            {
                ReturnUrl = returnUrl
            };

            return View("CreateUpdate", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddressCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await customerService.SaveAddressAsync(model.ToAddress(), User.GetSub());

            return Redirect(model.ReturnUrl);
        }

        public IActionResult Update(int id, string returnUrl)
        {
            var address = customerService.GetAddress(User.GetSub(), id);

            var model = address.ToAddressCreateUpdateViewModel();

            model.ReturnUrl = returnUrl;

            return View("CreateUpdate", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AddressCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var address = customerService.GetAddress(User.GetSub(), model.Id);

            if (address == null) return NotFound();

            model.ToAddress(address);

            await customerService.SaveAddressAsync(address, User.GetSub());

            return Redirect(model.ReturnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await customerService.DeleteAddressAsync(User.GetSub(), id);

            return RedirectToAction(nameof(Index));
        }
    }
}