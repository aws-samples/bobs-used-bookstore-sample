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
using Amazon.S3.Model;
using BOBS_Backend.DataModel;
using BOBS_Backend.ViewModel.ManageInventory;

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
        public IActionResult EditBookDetails( string bookname)
        {
            ViewData["user"]  = @User.Claims.FirstOrDefault(c => c.Type.Equals("cognito:username"))?.Value;

            DateTime time = DateTime.Now;
            if (bookname != null)
            {
                ViewData["Book"] = bookname;
            }
            ViewData["Types"] = _Inventory.GetTypes();
            ViewData["Publishers"] = _Inventory.GetAllPublishers();
            ViewData["Genres"] = _Inventory.GetGenres();
            ViewData["Conditions"] = _Inventory.GetConditions();
            return View();
        }

        [HttpPost] //Post Data from forms to tables
        public IActionResult EditBookDetails(BooksViewModel bookview )
        {
            bookview.UpdatedBy = @User.Claims.FirstOrDefault(c => c.Type.Equals("cognito:username"))?.Value;
            bookview.UpdatedOn = DateTime.Now;
            bookview.Active = true;
            if (ModelState.IsValid)
            {

                var status = _Inventory.AddToTables(bookview);

                if (status != 1)
                { 
  
                    ViewData["ErrorStatus"] = "Yes";
                                                          
                }

                ViewData["Types"] = _Inventory.GetTypes();
                ViewData["Publishers"] = _Inventory.GetAllPublishers();
                ViewData["Genres"] = _Inventory.GetGenres();
                ViewData["Conditions"] = _Inventory.GetConditions();
            }
            return View(bookview);
        }

        //Get list of all books in the inventory
        public IActionResult GetAllBooks()
        {

            var books = _Inventory.GetAllBooks(1 , "" , "");
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

       [HttpGet]
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
            ViewData["Types"] = _Inventory.GetVariantsOfTheSelectedBook(bookdetails.BookName);
            ViewData["status"] = "details";
            return View(books);

        }

        [HttpPost]
        public IActionResult BookDetails(string type , string BookName , FetchBooksViewModel book)
        {
            ViewData["Types"] = _Inventory.GetVariantsOfTheSelectedBook(BookName);
            ViewData["status"] = "List";
            ViewData["Books"] = _Inventory.GetRelevantBooks(BookName, type);
            var lis = _Inventory.GetRelevantBooks(BookName, type);
            book.BookType = lis[0].BookType.TypeName;
            book.publisher = lis[0].Publisher.Name;
            book.genre = lis[0].Genre.Name;
            return View(book);
        }
        /*
         * Order Repository contains all functions associated with Order Model
        
        public IActionResult SearchBeta(string style)
        {
            PagedSearchViewModel books = new PagedSearchViewModel();
            books.Books = _Inventory.GetAllBooks();
            var stats = _Inventory.DashBoard();
            ViewData["genre"] = stats[0].OrderByDescending(x => x.Value).First().Key;                  
            ViewData["type"] = stats[1].OrderByDescending(x => x.Value).First().Key;
            ViewData["publisher"] = stats[2].OrderByDescending(x => x.Value).First().Key;
            ViewData["name"] = stats[3].OrderByDescending(x => x.Value).First().Key;
            if (style != null)
            {
                books.ViewStyle = style;
            }

            else
            {
                books.ViewStyle = "Tabular";
            }
            return View(books);

            
        }
         */

        public IActionResult SearchBeta(string searchfilter, string searchby, int pageNum , string ViewStyle , string SortBy)
        {
            var stats = _Inventory.DashBoard();
            ViewData["genre"] = stats[0].OrderByDescending(x => x.Value).First().Key;
            ViewData["type"] = stats[1].OrderByDescending(x => x.Value).First().Key;
            ViewData["publisher"] = stats[2].OrderByDescending(x => x.Value).First().Key;
            ViewData["name"] = stats[3].OrderByDescending(x => x.Value).First().Key;

            if (pageNum == 0) pageNum++;

            if ((String.IsNullOrEmpty(searchby) && String.IsNullOrEmpty(searchfilter)))
            {
                var books = _Inventory.GetAllBooks(pageNum , ViewStyle , SortBy); 

                return View(books);
            }
            else
            {

                var books = _Inventory.SearchBeta(searchby, searchfilter , ViewStyle, SortBy, pageNum);
                

                return View(books);
            }          
           
        }

       [HttpPost]
        public IActionResult UpdateDetails(int BookId, string Condition)
        {
           var details =  _Inventory.UpdateDetails(BookId, Condition);
            
            return View(details);
        }

        public IActionResult Submitchanges(BookDetails details)
        {
            details.UpdatedBy = @User.Claims.FirstOrDefault(c => c.Type.Equals("cognito:username"))?.Value;
            details.UpdatedOn = DateTime.Now;
            _Inventory.PushDetails(details);

            return RedirectToAction("SearchBeta");
        }
       
        public IActionResult Dashboard(FetchBooksViewModel Book)
        {
            var stats = _Inventory.DashBoard();
            ViewData["ordes_top_genre"] = stats[0].First().Key;
            ViewData["ordes_top_genre_count"] = stats[0].First().Value;

            ViewData["ordes_top_type"] = stats[1].First().Key;
            ViewData["ordes_top_type_count"] = stats[1].First().Value;

            ViewData["ordes_top_publisher"] = stats[2].First().Key;
            ViewData["ordes_top_publisher_count"] = stats[2].First().Value;

            ViewData["ordes_top_name"] = stats[3].First().Key;
            ViewData["ordes_top_name_count"] = stats[3].First().Value;

            ViewData["orders_genre"] = stats[0];
            ViewData["orders_type"] = stats[1];
            ViewData["orders_publisher"] = stats[2];
            ViewData["orders_name"] = stats[3];

           
            var a = stats[4];

            List<int> InventoryStats = new List<int>();
            foreach (var i in a)
                InventoryStats.Add(i.Value);
            ViewData["Inventory"] = InventoryStats;

            var b = stats[5];
            List<int> OrdersStats = new List<int>();
            foreach (var i in b)
                OrdersStats.Add(i.Value);
            ViewData["Orders"] = OrdersStats;

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> AutoSuggest(string searchby)
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = _Inventory.autosuggest(term);

                return Ok(names);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
