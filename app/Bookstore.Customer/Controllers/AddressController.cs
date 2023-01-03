using Bookstore.Customer;
using Bookstore.Customer.Mappers;
using Bookstore.Customer.ViewModel.Address;
using Bookstore.Data;
using Bookstore.Domain.Customers;
using Bookstore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerSite.Controllers
{
    public class AddressController : Controller
    {
        private readonly IGenericRepository<Address> _addressRepository;
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly ICustomerService customerService;

        public AddressController(IGenericRepository<Address> addressRepository,
                                   IGenericRepository<Customer> customerRepository,
                                   ICustomerService customerService,
                                   ApplicationDbContext context)
        {
            _context = context;
            _customerRepository = customerRepository;
            this.customerService = customerService;
            _addressRepository = addressRepository;
        }

        public IActionResult Index()
        {
            var addresses = customerService.GetAddresses(User.GetSub());

            return View(addresses.ToAddressIndexViewModel());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddressCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await customerService.SaveAddressAsync(model.ToAddress(), User.GetSub());

            return RedirectToAction("Index", "Checkout");
        }

        public IActionResult Update(int id)
        {
            var address = customerService.GetAddress(User.GetSub(), id);

            if (address == null) return NotFound();

            return View(address.ToAddressCreateUpdateViewModel());
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

            return RedirectToAction("Index", "Checkout");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await customerService.DeleteAddressAsync(User.GetSub(), id);

            return RedirectToAction(nameof(Index));
        }
    }
}