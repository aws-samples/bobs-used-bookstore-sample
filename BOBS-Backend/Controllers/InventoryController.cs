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
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BOBS_Backend.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventory _Inventory;
        public DatabaseContext _context;
        private readonly ILogger<InventoryController> _logger;


        public InventoryController(IInventory Inventory , DatabaseContext context , ILogger<InventoryController> logger)
        {           
            _Inventory = Inventory;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult EditBookDetails(string BookName , string status)
        {
            _logger.LogInformation("Loading : Add New Book View");
            try
            {
                ViewData["user"] = @User.Claims.FirstOrDefault(c => c.Type.Equals("cognito:username"))?.Value;
                ViewData["Status"] = status;

            }

            catch( Exception e)
            {

                _logger.LogError(e, "Error in authentication @ Adding a new Book");
            }
                DateTime time = DateTime.Now;
            try
            {
                if (!string.IsNullOrEmpty(BookName))
                {
                    var temp = _context.Book.Where(b => b.Name == BookName).ToList()[0];
                    var variant = _Inventory.GetBookByID(temp.Book_Id);
                    ViewData["ISBN"] = variant.ISBN;
                    ViewData["Author"] = variant.Author;
                    ViewData["Summary"] = variant.Summary;
                }

                if (BookName != null)
                {
                    ViewData["Book"] = BookName;
                    
                }
                ViewData["Types"] = _Inventory.GetTypes();
                ViewData["Publishers"] = _Inventory.GetAllPublishers();
                ViewData["Genres"] = _Inventory.GetGenres();
                ViewData["Conditions"] = _Inventory.GetConditions();

            }
               catch(Exception e)
               {

                _logger.LogError(e,"Error in loading dropdownlists for Books Action Method");
               }

            ViewData["check"] = "yes";

            return View();
        }

        [HttpPost] //Post Data from forms to tables
        public IActionResult EditBookDetails(BooksViewModel bookview)
        {
            _logger.LogInformation("Posting new book details from form to Database ");
            bookview.UpdatedBy = @User.Claims.FirstOrDefault(c => c.Type.Equals("cognito:username"))?.Value;
            bookview.UpdatedOn = DateTime.Now;
            bookview.Active = true;
            if (String.IsNullOrEmpty(bookview.Author))
            {
                var temp = _context.Book.Where(b => b.Name == bookview.BookName).ToList()[0];
                var variant = _Inventory.GetBookByID(temp.Book_Id);
                bookview.Author = variant.Author;
                bookview.Summary = variant.Summary;
            }
            if (ModelState.IsValid)
            {

                var status = _Inventory.AddToTables(bookview);

                if (status != 1)
                {

                    ViewData["ErrorStatus"] = "Yes";



                    ViewData["Types"] = _Inventory.GetTypes();
                    ViewData["Publishers"] = _Inventory.GetAllPublishers();
                    ViewData["Genres"] = _Inventory.GetGenres();
                    ViewData["Conditions"] = _Inventory.GetConditions();
                    return View(bookview);
                }                
                    
                var temp = _context.Book.Where(b => b.Name == bookview.BookName).ToList()[0];
                var BookId = temp.Book_Id;
                return RedirectToAction("BookDetails", new { BookId });

            }

            return View (bookview);
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
            try
            {
                var status = _Inventory.AddPublishers(publisher);

                if (status == 0)
                {
                    return RedirectToAction("EditBookDetails");
                }
                if (status == 1)
                {
                    ViewData["Status"] = "A Publisher with the given Name already exists in the database";
                }
            }

            catch(Exception e)
            {
                _logger.LogError(e, "Error in adding a new publisher");
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
            try
            {
                var status = _Inventory.AddGenres(genre);

                if (status == 0)
                {
                    return RedirectToAction("EditBookDetails");
                }
                if (status == 1)
                {
                    ViewData["Status"] = "A Genre with the given Name already exists in the database";
                }
            }

            catch(Exception e)
            {
                _logger.LogError(e, "Error in adding a new genre");
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
            try
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
            }

            catch(Exception e)
            {
                _logger.LogError(e, "Error in adding a new type");
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
            try
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
            }

            catch(Exception e)
            {
                _logger.LogError(e, "Error in adding a new condition");
            }
            return View();

        }

       [HttpGet]
        public IActionResult BookDetails(long BookId)
        {
            _logger.LogInformation("Loading Book Details on Click in search page");
            
                FetchBooksViewModel books = new FetchBooksViewModel();
            try 
            { 
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
                books.Author = bookdetails.Author;
                books.ISBN = bookdetails.ISBN;
                books.Summary = bookdetails.Summary;

                ViewData["Types"] = _Inventory.GetFormatsOfTheSelectedBook(bookdetails.BookName);
                ViewData["Conditions"] = _Inventory.GetConditionsOfTheSelectedBook(bookdetails.BookName);
                ViewData["status"] = "details";
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in displaying Book Details");               
            }
            return View(books);
            

           
        }

        [HttpPost]
        public IActionResult BookDetails(string type , string condition_chosen , string BookName , FetchBooksViewModel book)
        {
            /*
                *  function to add details to the Book table
                
            if (String.IsNullOrEmpty(type))
            {
                type = _context.Book.Include(b => b.Type)
                        .Where(b => b.Name == BookName).ToList()[0].Type.TypeName;               
            }
            */
            _logger.LogInformation("Loading books to be displayed in table based on type and condition chosen");
            try
            {
                ViewData["Types"] = _Inventory.GetFormatsOfTheSelectedBook(BookName);
                ViewData["Conditions"] = _Inventory.GetConditionsOfTheSelectedBook(BookName);
                ViewData["status"] = "List";
                ViewData["Books"] = _Inventory.GetRelevantBooks(BookName, type, condition_chosen);
                var lis = _Inventory.GetRelevantBooks(BookName, type, condition_chosen);
                if(lis.Count == 0)
                {                  
                    var temp = _context.Book.Where(b => b.Name == BookName).ToList()[0];
                    var variant = _Inventory.GetBookByID(temp.Book_Id);
                    book.genre = variant.Genre.Name;
                    book.Author = variant.Author;
                    book.Summary = variant.Summary;
                    ViewData["status"] = "details";
                    ViewData["fetchstatus"] =  "Sorry , We don't currently have any relevant results for the given Combination";
                    return View(book);
                }
                book.BookType = lis[0].BookType.TypeName;
                book.publisher = lis[0].Publisher.Name;
                book.genre = lis[0].Genre.Name;
                book.front_url = lis[0].front_url;
                book.back_url = lis[0].back_url;
                book.Author = lis[0].Author;
                book.Author = lis[0].Author;
                book.ISBN = lis[0].ISBN;
                book.Summary = lis[0].Summary;
            }

            catch(Exception e)
            {
                _logger.LogError(e, "Error in loading table data for book details ");
            }
            return View(book);
        }
       
        public IActionResult SearchBeta(string searchfilter, string searchby, int pageNum, string ViewStyle, string SortBy , string ascdesc , string pagination)
        {
            _logger.LogInformation("Search Page");
            try
            {
                var stats = _Inventory.DashBoard();
                ViewData["genre"] = stats[0].OrderByDescending(x => x.Value).First().Key;
                ViewData["type"] = stats[1].OrderByDescending(x => x.Value).First().Key;
                ViewData["publisher"] = stats[2].OrderByDescending(x => x.Value).First().Key;
                ViewData["name"] = stats[3].OrderByDescending(x => x.Value).First().Key;
            }

            catch (Exception e)
            {
                _logger.LogError(e, "Error in loading search page dashboard");
            }

            try
            {
                if (pageNum == 0) pageNum++;

                if ((String.IsNullOrEmpty(searchby) || String.IsNullOrEmpty(searchfilter)))
                {

                    var books = _Inventory.GetAllBooks(1, "", "", "");
                    books.ViewStyle = (String.IsNullOrEmpty(ViewStyle)) ? "Tabular" : ViewStyle; 
                    books.SortBy = SortBy;
                    return View(books);
                }

                else
                {
                    //var books = _Inventory.SearchBeta(searchby, searchfilter, ViewStyle, SortBy, pageNum , ascdesc , pagination);
                    searchfilter = Regex.Replace(searchfilter, @"\s+", " ");
                    var books = _Inventory.SearchBooks(searchby, searchfilter, ViewStyle, SortBy, pageNum, ascdesc);
                    books.SortBy = SortBy;
                    return View(books);
                }
            }

            catch (Exception e)
            {
                _logger.LogError(e, "Error in loading search page");
            }

            return View();
        }

       [HttpPost]
        public IActionResult UpdateDetails(int BookId, string Condition)
        {
            _logger.LogInformation("Load Edit Book Details page with pre-filled values");
            try
            {
                var details = _Inventory.UpdateDetails(BookId, Condition);

                return View(details);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in fetching prefilled values for edit book details page");
            }

            return View();
        }

        public IActionResult Submitchanges(BookDetails details)
        {
            _logger.LogInformation("Posting the Edit Book form values to database");
            try
            {
                details.UpdatedBy = @User.Claims.FirstOrDefault(c => c.Type.Equals("cognito:username"))?.Value;
                details.UpdatedOn = DateTime.Now;
                _Inventory.PushDetails(details);

            }

            catch(Exception e)
            {
                _logger.LogError(e, "Error in posting Edited Book details to database");
            }
            return RedirectToAction("SearchBeta");
        }
       
        public IActionResult Dashboard(FetchBooksViewModel Book)
        {
            _logger.LogInformation("Dashboard Display");

            try
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
            }

            catch(Exception e)
            {
                _logger.LogError("Error in displaying dasboard statistics");
                return RedirectToAction("Error", "Home");
            }
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
            catch(Exception e)
            {
                _logger.LogError(e,"Error in loading autosearch results");
                return BadRequest();
            }
        }

    }
}
