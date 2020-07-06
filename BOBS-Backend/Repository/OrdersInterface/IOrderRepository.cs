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

        Task<Order> FindOrderById(long id);

        Task<ManageOrderViewModel> GetAllOrders(int pageNum);

        Task<ManageOrderViewModel> FilterList(string filterValue, string searchString,int pageNum);

        Task<ManageOrderViewModel> FilterOrderByOrderId(string searchString,int pageNum);



        Task<ManageOrderViewModel> FilterOrderByStatus(string searchString, int pageNum);



        Task<ManageOrderViewModel> FilterOrderByCustomerId(string searchString, int pageNum);

        Task<ManageOrderViewModel> FilterOrderByUsername(string searchString, int pageNum);

        Task<ManageOrderViewModel> FilterOrderByFirstName(string searchString, int pageNum);

        Task<ManageOrderViewModel> FilterOrderByEmail(string searchString, int pageNum);

        Task<ManageOrderViewModel> FilterOrderByLastName(string searchString, int pageNum);

        Task<ManageOrderViewModel> FilterOrderByPhone(string searchString, int pageNum);


        Task<ManageOrderViewModel> FilterOrderByState(string searchString, int pageNum);
        Task<ManageOrderViewModel> FilterOrderByZipCode(string searchString, int pageNum);
        Task<ManageOrderViewModel> FilterOrderByCity(string searchString, int pageNum);
     }
}
