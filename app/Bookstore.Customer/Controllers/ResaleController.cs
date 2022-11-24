using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using CustomerSite.Models.ViewModels;
using Bookstore.Domain.Customers;
using Bookstore.Data.Repository.Interface;
using Services;
using Bookstore.Domain.ReferenceData;
using Bookstore.Domain.Offers;

namespace CustomerSite.Controllers
{
    public class ResaleController : Controller
    {
        private const string ResaleStatusPending = "Pending Approval";
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Offer> _resaleRepository;
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly IReferenceDataService referenceDataService;

        public ResaleController(IMapper mapper, 
                                IGenericRepository<Offer> resaleRepository,
                                IGenericRepository<Customer> customerRepository,
                                SignInManager<CognitoUser> signInManager,
                                UserManager<CognitoUser> userManager,
                                IReferenceDataService referenceDataService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _customerRepository = customerRepository;
            _resaleRepository = resaleRepository;
            _mapper = mapper;
            this.referenceDataService = referenceDataService;
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
            if (_signInManager.IsSignedIn(User))
            {
                ViewData["Types"] = referenceDataService.GetReferenceData(ReferenceDataType.BookType);
                ViewData["Publishers"] = referenceDataService.GetReferenceData(ReferenceDataType.Publisher);
                ViewData["Genres"] = referenceDataService.GetReferenceData(ReferenceDataType.Genre);
                ViewData["Conditions"] = referenceDataService.GetReferenceData(ReferenceDataType.Condition);
                return View();
            }
            return NotFound("You must be signed in.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResaleViewModel resaleViewModel)
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var id = user.Attributes[CognitoAttribute.Sub.AttributeName];
                    var customer = _customerRepository.Get(id);
                    var resale = _mapper.Map<Offer>(resaleViewModel);
                    resale.Customer = customer;
                    resale.OfferStatus = OfferStatus.PendingApproval;
                    _resaleRepository.Add(resale);
                    _resaleRepository.Save();
                    return RedirectToAction(nameof(Index));
                }

                return View();
            }
            return NotFound("You must be signed in.");
        }
    }
}
