using Bookstore.Data.Repository.Interface;
using Bookstore.Domain;
using Bookstore.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public interface IOrderService
    {
        PaginatedList<Order> GetOrders(OrderStatus? orderStatus = null, int pageIndex = 1, int pageSize = 10);

        Order GetOrder(int id);

        IEnumerable<OrderDetail> GetOrderDetails(int id);

        Task SaveOrderAsync(Order order, string userName);
    }

    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> orderRepository;
        private readonly IGenericRepository<OrderDetail> orderDetailRepository;

        public OrderService(IGenericRepository<Order> orderRepository, IGenericRepository<OrderDetail> orderDetailRepository)
        {
            this.orderRepository = orderRepository;
            this.orderDetailRepository = orderDetailRepository;
        }

        public PaginatedList<Order> GetOrders(OrderStatus? orderStatus = null, int pageIndex = 1, int pageSize = 10)
        {
            return orderRepository.GetPaginated(filter: x => x.OrderStatus == orderStatus, pageIndex: pageIndex, pageSize: pageSize, includeProperties: new Expression<Func<Order, object>>[] { x => x.Customer });
        }

        public Order GetOrder(int id)
        {
            return orderRepository.Get2(x => x.Id == id, null, x => x.Customer, x => x.Address).SingleOrDefault();
        }

        public IEnumerable<OrderDetail> GetOrderDetails(int id)
        {
            return orderDetailRepository.Get2(x => x.Order.Id == id, null, x => x.Book, x => x.Book.BookType, x => x.Book.Condition, x => x.Book.Genre, x => x.Book.Publisher);
        }

        public async Task SaveOrderAsync(Order order, string userName)
        {
            if (order.IsNewEntity()) order.CreatedBy = userName;

            order.UpdatedOn = DateTime.UtcNow;

            orderRepository.AddOrUpdate(order);

            await orderRepository.SaveAsync();
        }
    }
}