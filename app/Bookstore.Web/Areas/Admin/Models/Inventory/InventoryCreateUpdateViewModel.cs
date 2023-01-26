using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookstore.Web.Areas.Admin.Models.Inventory
{
    public class InventoryCreateUpdateViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public int Year { get; set; }

        public string ISBN { get; set; }

        public IEnumerable<SelectListItem> Publishers { get; set; } = new List<SelectListItem>();
        public int SelectedPublisherId { get; set; }

        public IEnumerable<SelectListItem> BookTypes { get; set; } = new List<SelectListItem>();
        public int SelectedBookTypeId { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public int SelectedGenreId { get; set; }

        public IEnumerable<SelectListItem> BookConditions { get; set; } = new List<SelectListItem>();
        public int SelectedConditionId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; } = 1;

        public IFormFile CoverImage { get; set; }
        public string CoverImageUrl { get; set; }

        public string Summary { get; set; }
    }
}