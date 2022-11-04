using System;

namespace AdminSite.ViewModel.Inventory
{
    public class InventoryIndexViewModel
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

        public DateTime UpdatedOn { get; set; }
    }
}
