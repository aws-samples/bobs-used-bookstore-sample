using BOBS_Backend.Models.Order;
using BOBS_Backend.ViewModel.ManageOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository.OrdersInterface
{
    public interface IOrderRepository
    {

        /*
         * Order Detail Repository Interface
         */

        Task<Order> FindOrderById(long id);

        Task<ManageOrderViewModel> GetAllOrders(int pageNum);

        Task<List<Order>> FilterList(string filterValue, string searchString);

        Task<List<Order>> FilterOrderByOrderId(string searchString);

        Task<List<Order>> FilterOrderByCustomerId(string searchString);

        Task<List<Order>> FilterOrderByEmail(string searchString);

        Task<List<Order>> FilterOrderByState(string searchString);
    }
}
