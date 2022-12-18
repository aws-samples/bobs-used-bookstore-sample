namespace Bookstore.Customer.ViewModel
{
    public class ResaleViewModel
    {
        public long ResaleId { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string BookName { get; set; }
        public string PublisherName { get; set; }
        public string GenreName { get; set; }
        public string TypeName { get; set; }
        public decimal BookPrice { get; set; }
        public string ConditionName { get; set; }
    }
}