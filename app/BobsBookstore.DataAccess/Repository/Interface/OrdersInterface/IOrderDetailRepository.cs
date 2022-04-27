using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobsBookstore.Models.Orders;

namespace BobsBookstore.DataAccess.Repository.Interface.OrdersInterface
{
    public interface IOrderDetailRepository
    {
        Task<Order> CancelOrder(long id);

        Task<int> FindOrderDetailsRemovedCountAsync(long id);

        Task<Dictionary<string, string>> MakeOrderDetailInactive(long id, long orderId, int quantity);

        Task<OrderDetail> FindOrderDetailById(long id);

        Task<List<OrderDetail>> FindOrderDetailByOrderId(long orderId);

        IQueryable<OrderDetail> FilterOrderDetail(string filterValue, string searchString, string inBetween,
            string operand, string negate);
    }
}