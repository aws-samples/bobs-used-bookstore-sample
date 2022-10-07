using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Dtos;
using DataAccess.Repository.Interface;
using DataAccess.Repository.Interface.InventoryInterface;
using DataModels.Books;
using AdminSite.ViewModel.ResaleBooks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace AdminSite.Controllers { 

    [Authorize]
    public class ResaleController : Controller
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<Condition> _conditionRepository;
        private readonly IGenericRepository<Genre> _genreRepository;
        private readonly IInventory _inventory;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Price> _priceRepository;
        private readonly IGenericRepository<Publisher> _publisherRepository;
        private readonly IGenericRepository<Resale> _resaleRepository;
        private readonly IGenericRepository<ResaleStatus> _resaleStatusRepository;
        private readonly IGenericRepository<Type> _typeRepository;

        public ResaleController(IInventory inventory,
            IMapper mapper,
            IGenericRepository<Price> priceRepository,
            IGenericRepository<Condition> conditionRepository,
            IGenericRepository<Book> bookRepository,
            IGenericRepository<Type> typeRepository,
            IGenericRepository<Genre> genreRepository,
            IGenericRepository<Publisher> publisherRepository,
            IGenericRepository<ResaleStatus> resaleStatusRepository,
            IGenericRepository<Resale> resaleRepository)
        {
            _resaleRepository = resaleRepository;
            _resaleStatusRepository = resaleStatusRepository;
            _publisherRepository = publisherRepository;
            _genreRepository = genreRepository;
            _typeRepository = typeRepository;
            _bookRepository = bookRepository;
            _conditionRepository = conditionRepository;
            _priceRepository = priceRepository;
            _mapper = mapper;
            _inventory = inventory;
        }

        public IActionResult Index()
        {
            var resaleBooks = _resaleRepository.GetAll("ResaleStatus");

            return View(resaleBooks);
        }

        public IActionResult ApproveResale(long id)
        {
            var resaleBook = _resaleRepository.Get(c => c.Resale_Id == id, includeProperties: "ResaleStatus")
                .FirstOrDefault();
            resaleBook.ResaleStatus = _resaleStatusRepository.Get(c => c.Status == Constants.ResaleStatusApproved)
                .FirstOrDefault();
            _resaleRepository.Save();
            return RedirectToAction("Index");
        }

        public IActionResult RejectResale(long id)
        {
            var resaleBook = _resaleRepository.Get(c => c.Resale_Id == id, includeProperties: "ResaleStatus")
                .FirstOrDefault();
            resaleBook.ResaleStatus = _resaleStatusRepository.Get(c => c.Status == Constants.ResaleStatusRejected)
                .FirstOrDefault();
            _resaleRepository.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddResaleBookDetails(long id)
        {
            var resaleBooks = _resaleRepository.Get(c => c.Resale_Id == id, includeProperties: "ResaleStatus,Customer")
                .FirstOrDefault();
            var resaleViewModel = _mapper.Map<ResaleViewModel>(resaleBooks);
            return View(resaleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddResaleBookDetails(ResaleViewModel resaleViewModel)
        {
            resaleViewModel.ConditionName = resaleViewModel.ConditionName;
            var booksDto = _mapper.Map<BooksDto>(resaleViewModel);
            await _inventory.AddToTablesAsync(booksDto);
            var resale = _resaleRepository
                .Get(c => c.Resale_Id == resaleViewModel.Resale_Id, includeProperties: "ResaleStatus").FirstOrDefault();
            var resaleStatus = _resaleStatusRepository.Get(c => c.Status == Constants.ResaleStatusReceived)
                .FirstOrDefault();
            resale.ResaleStatus = resaleStatus;
            _resaleRepository.Update(resale);
            _resaleRepository.Save();
            return RedirectToAction("Index", new { resale });
        }

        public IActionResult Details(long id)
        {
            var resaleBooks = _resaleRepository.Get(c => c.Resale_Id == id, includeProperties: "ResaleStatus,Customer")
                .FirstOrDefault();
            var resaleViewModel = _mapper.Map<ResaleViewModel>(resaleBooks);
            return View(resaleViewModel);
        }

        public IActionResult PaymentDone(long id)
        {
            var resaleBook = _resaleRepository.Get(c => c.Resale_Id == id, includeProperties: "ResaleStatus")
                .FirstOrDefault();
            resaleBook.ResaleStatus = _resaleStatusRepository
                .Get(c => c.Status == Constants.ResaleStatusPaymentCompleted).FirstOrDefault();
            _resaleRepository.Update(resaleBook);
            _resaleRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
