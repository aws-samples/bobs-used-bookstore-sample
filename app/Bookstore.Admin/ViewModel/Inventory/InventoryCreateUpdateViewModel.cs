using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookstore.Admin.ViewModel.Inventory
{
    public class InventoryCreateUpdateViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string ISBN { get; set; }

        public IEnumerable<SelectListItem> Publishers { get; set; } = new List<SelectListItem>();
        public int SelectedPublisherId { get; set; }

        public IEnumerable<SelectListItem> BookTypes { get; set; } = new List<SelectListItem>();
        public int SelectedBookTypeId { get; set; }

        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public int SelectedGenreId { get; set; }

        public IEnumerable<SelectListItem> BookConditions { get; set; } = new List<SelectListItem>();
        public int SelectedConditionId { get; set; }

        public double? Price { get; set; }

        public int? Quantity { get; set; }

        public IFormFile FrontImage { get; set; }
        public string FrontImageUrl { get; set; }

        public IFormFile BackImage { get; set; }
        public string BackImageUrl { get; set; }

        public IFormFile LeftImage { get; set; }
        public string LeftImageUrl { get; set; }

        public IFormFile RightImage { get; set; }
        public string RightImageUrl { get; set; }

        public string Summary { get; set; }
    }
}