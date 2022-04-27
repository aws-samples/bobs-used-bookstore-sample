using System.Linq;
using BobsBookstore.DataAccess.Dtos;
using BobsBookstore.Models.Orders;

namespace BobsBookstore.DataAccess.Repository.Interface.OrdersInterface
{
    public interface IOrderRepository
    {
        public Order FindOrderById(long id);

        public ManageOrderDto GetAllOrders(int pageNum);

        public ManageOrderDto FilterList(string filterValue, string searchString, int pageNum);

        IQueryable<Order> FilterOrder(string filterValue, string searchString, string inBetween, string operand,
            string negate);
    }
}