using DataModels.Books;

namespace DataAccess.Dtos
{
    public class FullBookDto
    {
        public int LowestPrice { get; set; }

        public int TotalQuantity { get; set; }

        public Price Price { get; set; }
    }
}