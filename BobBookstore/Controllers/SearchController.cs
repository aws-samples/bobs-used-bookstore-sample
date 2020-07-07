using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobBookstore.Data;
using BobBookstore.Models.Book;
using BobBookstore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BobBookstore.Controllers
{
    public class SearchController : Controller
    {
        private readonly UsedBooksContext _context;

        public SearchController(UsedBooksContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync(string searchString)
        {

            var books = from b in _context.Book
                        select new BookViewModel()
                        {
                            BookId = b.Book_Id,
                            BookName = b.Name,
                            ISBN = b.ISBN,
                            GenreName = b.Genre.Name,
                            TypeName = b.Type.TypeName,
                            Url = b.Back_Url
                        };

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.BookName.Contains(searchString) || s.GenreName.Contains(searchString)
                                    || s.TypeName.Contains(searchString));
            }

            return View(await books.ToListAsync());
        }

        public IActionResult Detail(long id)
        {
            var book = from m in _context.Book
                       where m.Book_Id == id
                       select new BookViewModel()
                       {
                           BookName = m.Name,
                           ISBN = m.ISBN,
                           GenreName = m.Genre.Name,
                           TypeName = m.Type.TypeName,
                           Url = m.Back_Url
                       };

            return View(book.First());
        }
    }
}
