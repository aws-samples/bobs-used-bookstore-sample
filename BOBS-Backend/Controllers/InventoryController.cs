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
            ViewData["Types"] = _Inventory.GetTypes();
            ViewData["Publishers"] = _Inventory.GetAllPublishers();
            ViewData["Genres"] = _Inventory.GetGenres();
            ViewData["Conditions"] = _Inventory.GetConditions();
            return View();
        }

        [HttpPost]
        public IActionResult EditBookDetails(BooksViewModel bookview)
        {
            if (ModelState.IsValid)
            {

                var status = _Inventory.AddToTables(bookview);

                if (status == 1)
                {
                    return RedirectToAction("Search");
                }

                else
                {
                    ViewData["ErrorStatus"] = "Yes";
                    ViewData["Types"] = _Inventory.GetTypes();
                    ViewData["Publishers"] = _Inventory.GetAllPublishers();
                    ViewData["Genres"] = _Inventory.GetGenres();
                    ViewData["Conditions"] = _Inventory.GetConditions();
                    return View(bookview);                       
                }
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
            FetchBooksViewModel books = new FetchBooksViewModel();
            books.Books = _Inventory.GetAllBooks();
            return View(books);
        }


        [HttpPost]
        public IActionResult Search(FetchBooksViewModel filteredbooks)
        {
            if(filteredbooks.searchby == null)
            {
                return RedirectToAction("Search");
            }

            FetchBooksViewModel books = new FetchBooksViewModel();
            // books.Books = _Inventory.GetRequestedBooks(filteredbooks.searchby, filteredbooks.searchfilter);   
            books.Books = _Inventory.GetRequestedBooks(filteredbooks.searchby, filteredbooks.searchfilter);
            return View(books);

        }


        [HttpGet]
        public IActionResult AddPublishers()
        {

            ViewData["Status"] = "Please Enter the details of the publisher you wish to add to the database";
            return View();

        }

        [HttpPost]
        public  IActionResult AddPublishers(Publisher publisher)
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
        public IActionResult AddGenres(Genre genre)
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
        public IActionResult AddBookTypes(Models.Book.Type booktype)
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
        public IActionResult AddBookConditions(Condition bookcondition)
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


        public IActionResult BookDetails(long BookId)
        {

            FetchBooksViewModel books = new FetchBooksViewModel();
            var bookdetails = _Inventory.GetBookByID(BookId);
            books.publisher = bookdetails.Publisher.Name;
            books.genre = bookdetails.Genre.Name;
            books.BookType = bookdetails.BookType.TypeName;
            books.BookName = bookdetails.BookName;
            books.Books = _Inventory.GetDetails(BookId);
            books.front_url = bookdetails.front_url;
            books.back_url = bookdetails.back_url;
            books.left_url = bookdetails.left_url;
            books.right_url = bookdetails.right_url;
            return View(books);

        }

       
    }
}
