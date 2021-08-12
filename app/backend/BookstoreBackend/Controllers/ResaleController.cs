using BobsBookstore.DataAccess.Repository.Interface;
using BobsBookstore.Models.Books;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Type = BobsBookstore.Models.Books.Type;

namespace BookstoreBackend.Controllers
{
    public class ResaleController : Controller
    {
        private readonly IGenericRepository<Resale> _resaleRepository;
        private readonly IGenericRepository<ResaleStatus> _resaleStatusRepository;
        private readonly IGenericRepository<Publisher> _publisherRepository;
        private readonly IGenericRepository<Genre> _genreRepository;
        private readonly IGenericRepository<Type> _typeRepository;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<Condition> _conditionRepository;
        private readonly IGenericRepository<Price> _priceRepository;



        public ResaleController(IGenericRepository<Price> priceRepository, IGenericRepository<Condition> conditionRepository, IGenericRepository<Book> bookRepository, IGenericRepository<Type> typeRepository, IGenericRepository<Genre> genreRepository, IGenericRepository<Publisher> publisherRepository, IGenericRepository<ResaleStatus> resaleStatusRepository, IGenericRepository<Resale> resaleRepository)
        {

            _resaleRepository = resaleRepository;
            _resaleStatusRepository = resaleStatusRepository;
            _publisherRepository = publisherRepository;
            _genreRepository = genreRepository;
            _typeRepository = typeRepository;
            _bookRepository = bookRepository;
            _conditionRepository = conditionRepository;
            _priceRepository = priceRepository;
        }
        public IActionResult Index()
        {
            var resaleBooks = _resaleRepository.GetAll(includeProperties: "ResaleStatus");

            return View(resaleBooks);
        }

        public IActionResult ApproveResale(long id)
        {
           var resaleBook =  _resaleRepository.Get(c => c.Resale_Id == id, includeProperties: "ResaleStatus").FirstOrDefault();
            resaleBook.ResaleStatus = _resaleStatusRepository.Get(c => c.Status == Constants.ResaleStatusApproved).FirstOrDefault();
            _resaleRepository.Save();
            return RedirectToAction("Index");
        }

        public IActionResult RejectResale(long id)
        {
            var resaleBook = _resaleRepository.Get(c => c.Resale_Id == id, includeProperties: "ResaleStatus").FirstOrDefault();
            resaleBook.ResaleStatus = _resaleStatusRepository.Get(c => c.Status == Constants.ResaleStatusRejected).FirstOrDefault();
            _resaleRepository.Save();
            return RedirectToAction("Index");
        }

        public IActionResult ConfirmResale(long id)
        {
            var bookNew = new Book();
            var resaleBook = _resaleRepository.Get(c => c.Resale_Id == id, includeProperties: "ResaleStatus").FirstOrDefault();
            var publisher = _publisherRepository.Get(publisher => publisher.Name.ToLower() == resaleBook.PublisherName.ToLower()).FirstOrDefault();
            if (publisher == null)
            {
                publisher = new Publisher();
                publisher.Name = resaleBook.PublisherName;
                _publisherRepository.Add(publisher);
                _publisherRepository.Save();
                bookNew.Publisher = publisher;
            }
            else
                bookNew.Publisher = publisher;

            var type = _typeRepository.Get(type => type.TypeName.ToLower() == resaleBook.TypeName.ToLower()).FirstOrDefault();
            if (type == null)
            {
                type = new Type();
                type.TypeName = resaleBook.TypeName;
                _typeRepository.Add(type);
                _typeRepository.Save();
                bookNew.Type = type;
            }
            else
                bookNew.Type = type;

            var genre = _genreRepository.Get(genre => genre.Name.ToLower() == resaleBook.GenreName.ToLower()).FirstOrDefault();
            if (genre == null)
            {
                genre = new Genre();
                genre.Name = resaleBook.GenreName;
                _genreRepository.Add(genre);
                _genreRepository.Save();
                bookNew.Genre = genre;
            }
            else
                bookNew.Genre = genre;

            var condition = _conditionRepository.Get(condition => (condition.ConditionName).ToLower() == resaleBook.ConditionName.ToLower()).FirstOrDefault();
            if (condition == null)
            {
                condition = new Condition();
                condition.ConditionName = resaleBook.PublisherName;
                _conditionRepository.Add(condition);
                _conditionRepository.Save();
            }

            var price = new Price();
            price.Condition = condition;
            price.Quantity = 1;
            price.UpdatedOn = DateTime.Now.ToUniversalTime();
            price.Active = true;
            price.ItemPrice = resaleBook.BookPrice;




            bookNew.Publisher = publisher;
            bookNew.ISBN = resaleBook.ISBN;
            bookNew.Summary = resaleBook.Summary;
            bookNew.Author = resaleBook.Author;
            bookNew.Name = resaleBook.BookName;
            bookNew.LeftUrl = resaleBook.LeftUrl;
            bookNew.RightUrl = resaleBook.RightUrl;
            bookNew.FrontUrl = resaleBook.FrontUrl;
            bookNew.BackUrl = resaleBook.BackUrl;
            bookNew.AudioBookUrl = resaleBook.AudioBookUrl;
            _bookRepository.Add(bookNew);
            _resaleRepository.Remove(resaleBook);
            _bookRepository.Save();
            price.Book = bookNew;
            _resaleRepository.Save();
            _priceRepository.Add(price);
            _priceRepository.Save();


            return RedirectToAction("Index");
        }
        public IActionResult Details(long id)
        {
            var resaleBooks = _resaleRepository.Get(c => c.Resale_Id == id, includeProperties: "ResaleStatus,Customer").FirstOrDefault();

            return View(resaleBooks);
        }


        }
}
