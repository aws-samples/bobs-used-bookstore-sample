using System.Linq;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using DataAccess.Data;
using DataAccess.Repository.Interface;
using DataModels.Customers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerSite.Controllers
{
    public class AddressesController : Controller
    {
        private readonly IGenericRepository<Address> _addressRepository;

        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;

        public AddressesController(IGenericRepository<Address> addressRepository,
                                   IGenericRepository<Customer> customerRepository,
                                   ApplicationDbContext context,
                                   SignInManager<CognitoUser> signInManager,
                                   UserManager<CognitoUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var id = user.Attributes[CognitoAttribute.Sub.AttributeName];

            var temp = _addressRepository.Get(c => c.Customer.Customer_Id == id, includeProperties: "Customer");

            return View(temp);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Address_Id,AddressLine1,AddressLine2,City,State,Country,ZipCode")] Address address)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var id = user.Attributes[CognitoAttribute.Sub.AttributeName];
                var customer = _customerRepository.Get(id);

                address.Customer = customer;
                _addressRepository.Add(address);
                _addressRepository.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(address);
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
            if (id != address.Address_Id)
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
                    if (_addressRepository.Get(address.Address_Id) == null)
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
            var user = await _userManager.GetUserAsync(User);
            var customer_id = user.Attributes[CognitoAttribute.Sub.AttributeName];
            var customer = _customerRepository.Get(customer_id);
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
