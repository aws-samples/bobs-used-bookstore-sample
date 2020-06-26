using BOBS_Backend.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository
{
    public interface IOrderRepository
    {
        Order FindOrderById(long id);

        IEnumerable<Order> GetAllOrders();

        IEnumerable<Order> FilterList(string filterValue, string searchString);

        IEnumerable<Order> FilterOrderByOrderId(string searchString);

        IEnumerable<Order> FilterOrderByCustomerId(string searchString);

        IEnumerable<Order> FilterOrderByEmail(string searchString);

        IEnumerable<Order> FilterOrderByState(string searchString);
    }
}
