using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.DataAccess.Repository.Interface;
using BobsBookstore.Models.Books;
using BobsBookstore.Models.Customers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreFrontend.Controllers
{
    public class ResaleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<CognitoUser> _SignInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<Resale> _resaleRepository;
        private readonly IGenericRepository<ResaleStatus> _resaleStatusRepository;
        private const string ResaleStatusPending = "Pending Approval";


        public ResaleController(IGenericRepository<ResaleStatus> resaleStatusRepository, IGenericRepository<Resale> resaleRepository, IGenericRepository<Customer> customerRepository, ApplicationDbContext context, SignInManager<CognitoUser> SignInManager, UserManager<CognitoUser> userManager)
        {

            _context = context;
            _SignInManager = SignInManager;
            _userManager = userManager;
            _customerRepository = customerRepository;
            _resaleRepository = resaleRepository;
            _resaleStatusRepository = resaleStatusRepository;
         }
            public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var id = user.Attributes[CognitoAttribute.Sub.AttributeName];

            var resaleBooks = _resaleRepository.Get(c => c.Customer.Customer_Id == id, includeProperties: "Customer,ResaleStatus");

            return View(resaleBooks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Resale_Id,BookName,Author,GenreName,PublisherName,TypeName,ISBN, FrontUrl, BackUrl, LeftUrl, RightUrl, AudioBookUrl, Summary")] Resale resale)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var id = user.Attributes[CognitoAttribute.Sub.AttributeName];
                var customer = _customerRepository.Get(id);

                resale.Customer = customer;
                resale.ResaleStatus = _resaleStatusRepository.Get(c => c.Status == ResaleStatusPending).FirstOrDefault();
                _resaleRepository.Add(resale);
                _resaleRepository.Save();

                return RedirectToAction(nameof(Index));
            }
            return View(resale);
        }
    }
}
