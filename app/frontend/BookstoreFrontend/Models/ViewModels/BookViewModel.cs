using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BobsBookstore.Models.Books;

namespace BookstoreFrontend.Models.ViewModels
{
    public class BookViewModel
    {
        public long BookId { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        [Display(Name = "Title")]
        [DefaultValue("Title")]
        public string BookName { get; set; }

        [DefaultValue("Publisher not found")] public string PublisherName { get; set; }

        [DefaultValue("No Author")] public string Author { get; set; }

        public string ISBN { get; set; }

        [Display(Name = "Genre")] public string GenreName { get; set; }

        [Display(Name = "Type")] public string TypeName { get; set; }

        public List<Price> Prices { get; set; }
        public string Url { get; set; }

        [Display(Name = "$$")] public decimal MinPrice { get; set; }

        public string Summary { get; set; }
    }
}
