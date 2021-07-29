namespace BobsBookstore.Models.ViewModels
{
    public class CartViewModel
    {
        public long BookId { get; set; }
        public string BookName { get; set; }
        public long ISBN { get; set; }
        public string GenreName { get; set; }
        public string TypeName { get; set; }
        public decimal Prices { get; set; }
        public string Url { get; set; }

        public string CartItem_Id { get; set; }

        public int quantity { get; set; }
        public long PriceId { get; set; }

    }
}
