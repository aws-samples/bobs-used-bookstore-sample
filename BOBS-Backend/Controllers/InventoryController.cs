using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BOBS_Backend.Models;
using BOBS_Backend.ViewModel;


namespace BOBS_Backend.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventory _Inventory;
        public InventoryController(IInventory Inventory)
        {
            _Inventory = Inventory;
        }

        [HttpGet]
        public IActionResult EditBookDetails()
        {
            ViewData["Types"] = _Inventory.GetTypes();
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
                BOBS_Backend.Models.Book.Publisher publisher = new BOBS_Backend.Models.Book.Publisher();

                publisher.Publisher_Id = bookview.publisherId;
                publisher.Name = bookview.PublisherName;

                book.Name = bookview.BookName;
                book.Type = bookview.BookType;
                book.Genre = bookview.genre;
                book.ISBN = bookview.ISBN;
                book.Publisher = publisher;
                book.Front_Url = front_url;
                book.Back_Url = back_url;
                book.Left_Url = left_url;
                book.Right_Url = right_url;

                price.Condition = bookview.BookCondition;
                price.ItemPrice = bookview.price;
                price.Book = book;
                price.Quantiy = bookview.quantity;
                price.Price_Id = random.Next(1000000, 9000000);

                publisher.Publisher_Id = bookview.publisherId;
                publisher.Name = bookview.PublisherName;

                _Inventory.SaveBook(book);
                _Inventory.SavePrice(price);
                _Inventory.SavePublisherDetails(publisher);
                return RedirectToAction("Search");
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

            return View(books);

        }


        [HttpPost]
        public IActionResult Search(BOBS_Backend.ViewModel.FetchBooksViewModel filteredbooks)
        {


            BOBS_Backend.ViewModel.FetchBooksViewModel books = new BOBS_Backend.ViewModel.FetchBooksViewModel();
            books.Books = _Inventory.GetRequestedBooks(filteredbooks.BookName, filteredbooks.publisher, filteredbooks.condition, filteredbooks.BookType);

            return View(books);

        }


        [HttpGet]
        public IActionResult AddPublishers()
        {
            

            return View();

        }

        [HttpPost]
        public IActionResult AddPublishers(BOBS_Backend.Models.Book.Publisher publisher)
        {
            _Inventory.AddPublishers(publisher);

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
            _Inventory.AddGenres(genre);

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
            _Inventory.AddBookTypes(booktype);

            return View();

        }

        public IActionResult GetAllTypes()
        {

            _Inventory.GetTypes();
            return View();

        }

    }
}
