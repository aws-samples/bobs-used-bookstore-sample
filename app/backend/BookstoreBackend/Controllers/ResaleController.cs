using BobsBookstore.DataAccess.Repository.Interface;
using BobsBookstore.Models.Books;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            var publisher = _publisherRepository.Get(publisher => publisher.Name == resaleBook.PublisherName).FirstOrDefault();
            if (publisher == null)
            {
                var publisherNew = new Publisher();
                publisherNew.Name = resaleBook.PublisherName;
                _publisherRepository.Add(publisherNew);
                _publisherRepository.Save();
                bookNew.Publisher = publisherNew;
            }
            else
                bookNew.Publisher = publisher;

            var type = _typeRepository.Get(type => type.TypeName == resaleBook.TypeName).FirstOrDefault();
            if (type == null)
            {
                var typeNew = new Type();
                typeNew.TypeName = resaleBook.TypeName;
                _typeRepository.Add(typeNew);
                _typeRepository.Save();
                bookNew.Type = typeNew;
            }

            var genre = _genreRepository.Get(genre => genre.Name == resaleBook.GenreName).FirstOrDefault();
            if (genre == null)
            {
                var genreNew = new Genre();
                genreNew.Name = resaleBook.GenreName;
                _genreRepository.Add(genreNew);
                _genreRepository.Save();
                bookNew.Genre = genreNew;
            }
            else
                bookNew.Genre = genre;

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
            _resaleRepository.Save();


            return RedirectToAction("Index");
        }
        public IActionResult Details(long id)
        {
            var resaleBooks = _resaleRepository.Get(c => c.Resale_Id == id, includeProperties: "ResaleStatus,Customer").FirstOrDefault();

            return View(resaleBooks);
        }


        }
}
