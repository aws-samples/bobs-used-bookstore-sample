using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Bookstore.Web.ViewModel;

namespace Bookstore.Web.ViewModel.Search
{
    public class SearchIndexViewModel : PaginatedViewModel
    {
        public string SearchString { get; set; }

        public string SortBy { get; set; }

        public List<SearchIndexItemViewModel> Books { get; set; } = new List<SearchIndexItemViewModel>();
    }

    public class SearchIndexItemViewModel
    {
        public int BookId { get; set; }

        [Display(Name = "Title")]
        [DefaultValue("Title")]
        public string BookName { get; set; }

        [DefaultValue("Publisher not found")]
        public string PublisherName { get; set; }

        [DefaultValue("No Author")]
        public string Author { get; set; }

        [Display(Name = "Genre")]
        public string GenreName { get; set; }

        [Display(Name = "Type")]
        public string TypeName { get; set; }

        [Display(Name = "Condition")]
        public string ConditionName { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "$$")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}