using BobsBookstore.Models.Orders;

namespace BobsBookstore.DataAccess.Dtos
{
    public class FilterOrdersDto
    {
        public Order Order { get; set; }

        public int Severity { get; set; }
    }
}
