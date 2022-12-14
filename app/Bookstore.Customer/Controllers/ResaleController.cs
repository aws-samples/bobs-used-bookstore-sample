using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Bookstore.Domain.Customers;
using Bookstore.Data.Repository.Interface;
using Services;
using Bookstore.Domain.ReferenceData;
using Bookstore.Domain.Offers;
using Bookstore.Customer;
using Bookstore.Customer.ViewModel;

namespace CustomerSite.Controllers
{
    public class ResaleController : Controller
    {
        private const string ResaleStatusPending = "Pending Approval";
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Offer> _resaleRepository;
        private readonly IReferenceDataService referenceDataService;

        public ResaleController(IMapper mapper, 
                                IGenericRepository<Offer> resaleRepository,
                                IGenericRepository<Customer> customerRepository,
                                IReferenceDataService referenceDataService)
        {
            _customerRepository = customerRepository;
            _resaleRepository = resaleRepository;
            _mapper = mapper;
            this.referenceDataService = referenceDataService;
        }

        public async Task<IActionResult> Index()
        {
            var resaleBooks
                = _resaleRepository.Get(c => c.Customer.Sub == User.GetUserId(),
                                        includeProperties: "Customer");
            return View(resaleBooks);
        }

        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
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
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var customer = _customerRepository.Get(User.GetUserId());
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
