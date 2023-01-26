namespace Bookstore.Web.Areas.Admin.Models.Inventory
{
    public class InventoryDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public int Year { get; set; }

        public string Publisher { get; set; }

        public string BookType { get; set; }

        public string Genre { get; set; }

        public string Condition { get; set; }

        public string CoverImageUrl { get; set; }

        public string Summary { get; set; }

        public string ISBN { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
