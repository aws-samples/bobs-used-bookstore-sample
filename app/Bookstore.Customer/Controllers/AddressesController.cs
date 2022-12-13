using System.Linq;
using System.Threading.Tasks;
using Bookstore.Customer;
using Bookstore.Customer.Mappers;
using Bookstore.Customer.ViewModel.Addresses;
using Bookstore.Data.Data;
using Bookstore.Data.Repository.Interface;
using Bookstore.Domain.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerSite.Controllers
{
    public class AddressesController : Controller
    {
        private readonly IGenericRepository<Address> _addressRepository;
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Customer> _customerRepository;

        public AddressesController(IGenericRepository<Address> addressRepository,
                                   IGenericRepository<Customer> customerRepository,
                                   ApplicationDbContext context)
        {
            _context = context;
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
        }

        public async Task<IActionResult> Index()
        {
            var temp = _addressRepository.Get(c => c.Customer.Id == User.GetUserId(), includeProperties: "Customer");

            return View(temp);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddressCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var address = model.ToAddress(User.GetUserId());

                _addressRepository.Add(address);
                _addressRepository.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public IActionResult Edit(long? id)
        {
            if (id == null)
                return NotFound();

            var address = _addressRepository.Get(id);
            if (address == null)
                return NotFound();

            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, [Bind("Address_Id,AddressLine1,AddressLine2,City,State,Country,ZipCode")] Address address)
        {
            if (id != address.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _addressRepository.Update(address);
                    _addressRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_addressRepository.Get(address.Id) == null)
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(address);
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
            var customer = _customerRepository.Get(User.GetUserId());
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
