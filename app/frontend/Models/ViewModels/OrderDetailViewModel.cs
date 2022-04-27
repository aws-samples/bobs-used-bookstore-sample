namespace BobsBookstore.Models.ViewModels
{
    public class OrderDetailViewModel
    {
        public string Bookname { get; set; }
        public long Book_Id { get; set; }
        public int Quantity { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
    }
}