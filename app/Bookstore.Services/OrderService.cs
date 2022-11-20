using Bookstore.Data.Repository.Interface;
using Bookstore.Domain;
using Bookstore.Domain.Books;
using Bookstore.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public interface IOrderService
    {
        PaginatedList<Order> GetOrders(string userName, int index, int count);

    }

    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> orderRepository;

        public OrderService(IGenericRepository<Order> orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public PaginatedList<Order> GetOrders(string userName, int index, int count)
        {
            return orderRepository.GetPaginated(pageIndex: index, pageSize: count);
        }
    }
}
