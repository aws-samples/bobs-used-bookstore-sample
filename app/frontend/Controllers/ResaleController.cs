using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.DataAccess.Repository.Interface;
using BobsBookstore.DataAccess.Repository.Interface.InventoryInterface;
using BobsBookstore.Models.Books;
using BobsBookstore.Models.Customers;
using BookstoreFrontend.Models.ViewModels;

namespace BookstoreFrontend.Controllers
{
    public class ResaleController : Controller
    {
        private const string ResaleStatusPending = "Pending Approval";
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IInventory _inventory;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Resale> _resaleRepository;
        private readonly IGenericRepository<ResaleStatus> _resaleStatusRepository;
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;


        public ResaleController(IMapper mapper, IInventory inventory,
                                IGenericRepository<ResaleStatus> resaleStatusRepository,
                                IGenericRepository<Resale> resaleRepository,
                                IGenericRepository<Customer> customerRepository,
                                ApplicationDbContext context,
                                SignInManager<CognitoUser> signInManager,
                                UserManager<CognitoUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _customerRepository = customerRepository;
            _resaleRepository = resaleRepository;
            _resaleStatusRepository = resaleStatusRepository;
            _inventory = inventory;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var id = user.Attributes[CognitoAttribute.Sub.AttributeName];
            var resaleBooks
                = _resaleRepository.Get(c => c.Customer.Customer_Id == id,
                                        includeProperties: "Customer,ResaleStatus");
            return View(resaleBooks);
        }

        public IActionResult Create()
        {
            ViewData["Types"] = _inventory.GetTypes();
            ViewData["Publishers"] = _inventory.GetAllPublishers();
            ViewData["Genres"] = _inventory.GetGenres();
            ViewData["Conditions"] = _inventory.GetConditions();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResaleViewModel resaleViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var id = user.Attributes[CognitoAttribute.Sub.AttributeName];
                var customer = _customerRepository.Get(id);
                var resale = _mapper.Map<Resale>(resaleViewModel);
                resale.Customer = customer;
                resale.ResaleStatus
                    = _resaleStatusRepository.Get(c => c.Status == ResaleStatusPending).FirstOrDefault();
                _resaleRepository.Add(resale);
                _resaleRepository.Save();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
