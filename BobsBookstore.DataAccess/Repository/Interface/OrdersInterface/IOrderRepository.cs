using BobsBookstore.DataAccess.Dtos;
using BobsBookstore.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobsBookstore.DataAccess.Repository.Interface.OrdersInterface
{
    public interface IOrderRepository
    {

        /*
         * Order Detail Repository Interface
         */

        public Order FindOrderById(long id);

        public ManageOrderDto GetAllOrders(int pageNum);

        public ManageOrderDto FilterList(string filterValue, string searchString, int pageNum);


        IQueryable<Order> FilterOrder(string filterValue, string searchString, string inBetween, string operand, string negate);

     }
}
