using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOBS_Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using BOBS_Backend.DataModel;
using BOBS_Backend.Models;
using Microsoft.EntityFrameworkCore.Internal;
using BOBS_Backend.Database;

namespace BOBS_Backend.ViewModel
{
    public class BooksViewModel
    {

        public String BookName { get; set; }
        public long ISBN { get; set; }
        public long BookId { get; set; }
        public long publisherId { get; set; }
        public string PublisherName { get; set; }
        public BOBS_Backend.Models.Book.Type BookType { get; set; }
        public BOBS_Backend.Models.Book.Genre genre { get; set; }
        public BOBS_Backend.Models.Book.Condition BookCondition { get; set; }
        public long PriceId { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public String Name { get; set; }
        public IFormFile FrontPhoto { get; set; }
        public IFormFile BackPhoto { get; set; }
        public IFormFile RearPhoto { get; set; }
        public IFormFile LeftSidePhoto { get; set; }
        public IFormFile RightSidePhoto { get; set; }

        public List<string> booktypes { get; set; }

    }
}
