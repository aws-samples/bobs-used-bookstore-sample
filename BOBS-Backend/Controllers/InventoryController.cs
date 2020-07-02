using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BOBS_Backend.Models;
using BOBS_Backend.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using BOBS_Backend.Database;
using BOBS_Backend.Models.Book;

namespace BOBS_Backend.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventory _Inventory;
        public DatabaseContext _context;
        public InventoryController(IInventory Inventory , DatabaseContext context)
        {
            _Inventory = Inventory;
            _context = context;
        }

        [HttpGet]
        public IActionResult EditBookDetails()
        {
           // ViewBag.Types = new SelectList(_context.Publisher, "Publisher_Id", "Name");
            ViewData["Types"] = _Inventory.GetTypes();
            ViewData["Publishers"] = _Inventory.GetAllPublishers();
            ViewData["Genres"] = _Inventory.GetGenres();
            ViewData["Conditions"] = _Inventory.GetConditions();
            return View();
        }

        [HttpPost]
        public IActionResult EditBookDetails(ViewModel.BooksViewModel bookview)
        {
            if (ModelState.IsValid)
            {

                var front_url = _Inventory.UploadtoS3(bookview.FrontPhoto).Result;
                var back_url = _Inventory.UploadtoS3(bookview.BackPhoto).Result;
                var left_url = _Inventory.UploadtoS3(bookview.LeftSidePhoto).Result;
                var right_url = _Inventory.UploadtoS3(bookview.RightSidePhoto).Result;

                Random random = new Random();
                BOBS_Backend.Models.Book.Book book = new BOBS_Backend.Models.Book.Book();
                BOBS_Backend.Models.Book.Price price = new BOBS_Backend.Models.Book.Price();
               // BOBS_Backend.Models.Book.Publisher publisher = new BOBS_Backend.Models.Book.Publisher();

                var publisherdata = _context.Publisher.Where(publisher => publisher.Name == bookview.PublisherName).ToList();
                var genredata = _context.Genre.Where(genre => genre.Name == bookview.genre).ToList();
                var typedata = _context.Type.Where(type => type.TypeName == bookview.BookType).ToList();
                var conditiondata = _context.Condition.Where(condition => condition.ConditionName == bookview.BookCondition).ToList();

                book.Name = bookview.BookName;
                book.Type = typedata[0];
                book.Genre = genredata[0];
                book.ISBN = bookview.ISBN;
                book.Publisher = publisherdata[0];
                book.Front_Url = front_url;
                book.Back_Url = back_url;
                book.Left_Url = left_url;
                book.Right_Url = right_url;

                price.Condition = conditiondata[0];
                price.ItemPrice = bookview.price;
                price.Book = book;
                price.Quantiy = bookview.quantity;

                var books = _context.Book.Where(temp => temp.Name == book.Name && temp.Type == book.Type && temp.Publisher == book.Publisher && temp.Genre == book.Genre).ToList();
                if (books.Count == 0)
                {
                   _Inventory.SaveBook(book);
                    
                }

                else
                {

                    var prices = _context.Price.Where(p => p.Condition == price.Condition && p.Book == book).ToList();
                        if (prices.Count == 0)
                    {
                        _Inventory.SavePrice(price);
                    }
                    
                        else
                    {
                       var output =  _context.Price.SingleOrDefault(p => p.Condition == price.Condition && p.Book == book);
                       output.Quantiy = bookview.quantity;
                         _context.SaveChanges();
                    }
                       
                }

                return RedirectToAction("Home/WelcomePage");
            }
            return View(bookview);
        }

        public IActionResult GetAllBooks()
        {

            var books = _Inventory.GetAllBooks();
            return View(books);
        }

        [HttpGet]
        public IActionResult Search()
        {
            BOBS_Backend.ViewModel.FetchBooksViewModel books = new BOBS_Backend.ViewModel.FetchBooksViewModel();
            books.Books = _Inventory.GetAllBooks();
            

            ViewData["Types"] = _Inventory.GetTypes();
            ViewData["Publishers"] = _Inventory.GetAllPublishers();
            ViewData["Genres"] = _Inventory.GetGenres();
            ViewData["Conditions"] = _Inventory.GetConditions();
            return View(books);
        }


        [HttpPost]
        public IActionResult Search(BOBS_Backend.ViewModel.FetchBooksViewModel filteredbooks)
        {
            if(filteredbooks.searchfilter == "")
            {
                return RedirectToAction("Search");
            }

            if (filteredbooks.searchfilter != "" && filteredbooks.publisher == null && filteredbooks.BookType == null && filteredbooks.BookName == null && filteredbooks.BookType == null && filteredbooks.Condition == null && filteredbooks.genre ==null)
            {
                BOBS_Backend.ViewModel.FetchBooksViewModel book = new BOBS_Backend.ViewModel.FetchBooksViewModel();
                book.Books = _Inventory.GetAllBooks();
                book.searchfilter = filteredbooks.searchfilter;
                ViewData["Types"] = _Inventory.GetTypes();
                ViewData["Publishers"] = _Inventory.GetAllPublishers();
                ViewData["Genres"] = _Inventory.GetGenres();
                ViewData["Conditions"] = _Inventory.GetConditions();
                return View(book);
            }

            BOBS_Backend.ViewModel.FetchBooksViewModel books = new BOBS_Backend.ViewModel.FetchBooksViewModel();
            books.Books = _Inventory.GetRequestedBooks(filteredbooks.BookName, filteredbooks.publisher, filteredbooks.Condition, filteredbooks.BookType , filteredbooks.genre , filteredbooks.searchfilter);

            ViewData["Types"] = _Inventory.GetTypes();
            ViewData["Publishers"] = _Inventory.GetAllPublishers();
            ViewData["Genres"] = _Inventory.GetGenres();
            ViewData["Conditions"] = _Inventory.GetConditions();
            return View(books);

        }


        [HttpGet]
        public IActionResult AddPublishers()
        {

            ViewData["Status"] = "Please Enter the details of the publisher you wish to add to the database";
            return View();

        }

        [HttpPost]
        public  IActionResult AddPublishers(BOBS_Backend.Models.Book.Publisher publisher)
        {
            var status = _Inventory.AddPublishers(publisher);

            if (status == 0)
            {
                return RedirectToAction("EditBookDetails");
            }
            if(status == 1)
            {
                ViewData["Status"] = "A Publisher with the given Name already exists in the database";
            }

            return View();

        }

        [HttpGet]
        public IActionResult AddGenres()
        {


            return View();

        }

        [HttpPost]
        public IActionResult AddGenres(BOBS_Backend.Models.Book.Genre genre)
        {
            var status =_Inventory.AddGenres(genre);

            if (status == 0)
            {
                return RedirectToAction("EditBookDetails");
            }
            if (status == 1)
            {
                ViewData["Status"] = "A Genre with the given Name already exists in the database";
            }

            return View();

        }

        [HttpGet]
        public IActionResult AddBookTypes()
        {


            return View();

        }

        [HttpPost]
        public IActionResult AddBookTypes(BOBS_Backend.Models.Book.Type booktype)
        {
           var status = _Inventory.AddBookTypes(booktype);

            if (status == 0)
            {
                return RedirectToAction("EditBookDetails");
            }
            if (status == 1)
            {
                ViewData["Status"] = "A BookType with the given Name already exists in the database";
            }

            return View();

        }

        [HttpGet]
        public IActionResult AddBookConditions()
        {
            return View();

        }

        [HttpPost]
        public IActionResult AddBookConditions(BOBS_Backend.Models.Book.Condition bookcondition)
        {
            var status = _Inventory.AddBookConditions(bookcondition);

            if (status == 0)
            {
                return RedirectToAction("EditBookDetails");
            }
            if (status == 1)
            {
                ViewData["Status"] = "A BookCondition with the given Name already exists in the database";
            }

            return View();

        }

        public IActionResult GetAllTypes()
        {

            _Inventory.GetTypes();
            return View();

        }

    }
}
