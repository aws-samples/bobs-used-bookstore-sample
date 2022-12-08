using Bookstore.Services.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Bookstore.Admin.ViewModel.Inventory
{
    public class InventoryIndexViewModel : PaginatedViewModel
    {
        public List<InventoryIndexListItemViewModel> Items { get; set; } = new List<InventoryIndexListItemViewModel>();

        public InventoryFilters Filters { get; set; } = new InventoryFilters();

        public IEnumerable<SelectListItem> Publishers { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> BookTypes { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> Genres { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> BookConditions { get; set; } = new List<SelectListItem>();
    }

    public class InventoryIndexListItemViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public int Year { get; set; }

        public string Publisher { get; set; }

        public string Genre { get; set; }

        public string BookType { get; set; }

        public string Condition { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}