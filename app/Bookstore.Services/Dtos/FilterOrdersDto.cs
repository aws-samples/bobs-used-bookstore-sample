using Bookstore.Domain.Orders;

namespace DataAccess.Dtos
{
    public class FilterOrdersDto
    {
        public Order Order { get; set; }

        public int Severity { get; set; }
    }
}