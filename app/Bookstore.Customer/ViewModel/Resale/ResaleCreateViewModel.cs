using Bookstore.Domain.ReferenceData;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Bookstore.Web.ViewModel.Resale
{
    public class ResaleCreateViewModel
    {
        public IEnumerable<SelectListItem> BookTypes { get; internal set; }
        public IEnumerable<SelectListItem> Publishers { get; internal set; }
        public IEnumerable<SelectListItem> Genres { get; internal set; }
        public IEnumerable<SelectListItem> Conditions { get; internal set; }

        public int SelectedBookTypeId { get; set; }

        public int SelectedPublisherId { get; set; }

        public int SelectedGenreId { get; set; }

        public int SelectedConditionId { get; set; }

        public decimal BookPrice { get; set; }

        public string BookName { get; set; }

        public string Author { get; set; }

        public string ISBN { get; set; }
    }
}
