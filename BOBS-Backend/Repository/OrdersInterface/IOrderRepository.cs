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

        ManageOrderViewModel RetrieveViewModel(string filterValue, string searchString, int pageNum, int totalPages, int[] pages, List<Order> order);

        IQueryable<Order> GetBaseOrderQuery();

        int GetTotalPages(IQueryable<Order> orderQuery);

        Task<ManageOrderViewModel> RetrieveFilterViewModel(IQueryable<Order> filterQuery, int totalPages, int pageNum, string filterValue, string searchString);

        Task<Order> FindOrderById(long id);

        Task<ManageOrderViewModel> GetAllOrders(int pageNum);

        Task<ManageOrderViewModel> FilterList(string filterValue, string searchString,int pageNum);




     }
}
