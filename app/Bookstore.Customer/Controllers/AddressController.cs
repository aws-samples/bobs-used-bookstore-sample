using Bookstore.Customer;
using Bookstore.Customer.Mappers;
using Bookstore.Customer.ViewModel.Addresses;
using Bookstore.Data.Data;
using Bookstore.Data.Repository.Interface;
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

            return View(addresses);
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

        public IActionResult Delete(long? id)
        {
            if (id == null)
                return NotFound();

            var address = _addressRepository.Get(id);
            if (address == null)
                return NotFound();

            return View(address);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            var address = _addressRepository.Get(id);
            _addressRepository.Remove(address);
            _addressRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SwitchToPrime(long id)
        {
            //change origin to not prime
            var customer = _customerRepository.Get(User.GetSub());
            var addresses
                = from c in _context.Address
                  where c.Customer == customer && c.IsPrimary == true
                  select c;
            foreach (var item in addresses)
            {
                item.IsPrimary = false;
            }

            // change to prime
            var address = await _context.Address.FindAsync(id);
            address.IsPrimary = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
