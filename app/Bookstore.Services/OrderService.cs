using Bookstore.Data.Repository.Interface;
using Bookstore.Domain;
using Bookstore.Domain.Books;
using Bookstore.Domain.Carts;
using Bookstore.Domain.Customers;
using Bookstore.Domain.Orders;
using Bookstore.Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public interface IOrderService
    {
        PaginatedList<Order> GetOrders(OrderFilters filters, int pageIndex = 1, int pageSize = 10);

        IEnumerable<Order> GetOrders(string sub);

        Order GetOrder(int id);

        IEnumerable<OrderItem> GetOrderDetails(int id);

        Task SaveOrderAsync(Order order, string userName);

        Task<int> CreateOrderAsync(string shoppingCartId, string sub, int selectedAddressId);

        Task CancelOrderAsync(int orderId, string sub);
    }

    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> orderRepository;
        private readonly IGenericRepository<OrderItem> orderItemRepository;
        private readonly IGenericRepository<ShoppingCartItem> shoppingCartItemRepository;
        private readonly IGenericRepository<Customer> customerRepository;
        private readonly IGenericRepository<Book> bookRepository;

        public OrderService(
            IGenericRepository<Order> orderRepository, 
            IGenericRepository<OrderItem> orderItemRepository, 
            IGenericRepository<ShoppingCartItem> shoppingCartItemRepository,
            IGenericRepository<Customer> customerRepository,
            IGenericRepository<Book> bookRepository)
        {
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
            this.shoppingCartItemRepository = shoppingCartItemRepository;
            this.customerRepository = customerRepository;
            this.bookRepository = bookRepository;
        }

        public PaginatedList<Order> GetOrders(OrderFilters filters, int pageIndex = 1, int pageSize = 10)
        {
            var filterExpressions = new List<Expression<Func<Order, bool>>>();

            if (filters.OrderStatusFilter.HasValue)
            {
                filterExpressions.Add(x => x.OrderStatus == filters.OrderStatusFilter);
            }

            return orderRepository.GetPaginated(filterExpressions, pageIndex: pageIndex, pageSize: pageSize, includeProperties: new Expression<Func<Order, object>>[] { x => x.Customer });
        }

        public IEnumerable<Order> GetOrders(string sub)
        {
            var orders = orderRepository.Get2(x => x.Customer.Sub == sub);
            var orderIds = orders.Select(x => x.Id);
            var orderItems = orderItemRepository.Get2(x => orderIds.Contains(x.OrderId), null, x => x.Book);

            orders.ToList().ForEach(x =>
            {
                x.OrderItems = orderItems.Where(y => y.OrderId == x.Id).ToList();
            });

            return orders;
        }

        public Order GetOrder(int id)
        {
            var order = orderRepository.Get2(x => x.Id == id, null, x => x.Customer, x => x.Address).SingleOrDefault();
            var orderItems = orderItemRepository.Get2(x => x.Order.Id == id, null, x => x.Book, x => x.Book.BookType, x => x.Book.Condition, x => x.Book.Genre, x => x.Book.Publisher);

            order.OrderItems = orderItems;

            return order;
        }

        public IEnumerable<OrderItem> GetOrderDetails(int id)
        {
            return orderItemRepository.Get2(x => x.Order.Id == id, null, x => x.Book, x => x.Book.BookType, x => x.Book.Condition, x => x.Book.Genre, x => x.Book.Publisher);
        }

        public async Task SaveOrderAsync(Order order, string userName)
        {
            if (order.IsNewEntity()) order.CreatedBy = userName;

            order.UpdatedOn = DateTime.UtcNow;

            orderRepository.AddOrUpdate(order);

            await orderRepository.SaveAsync();
        }

        public async Task<int> CreateOrderAsync(string shoppingCartId, string sub, int selectedAddressId)
        {
            var shoppingCartItems = shoppingCartItemRepository.Get2(x => x.ShoppingCart.CorrelationId == shoppingCartId && x.WantToBuy == true);
            var customer = customerRepository.Get2(x => x.Sub == sub).Single();

            var order = new Order
            {
                AddressId = selectedAddressId,
                Customer = customer,
                OrderStatus = OrderStatus.Pending,
                CreatedBy = sub
            };

            await orderRepository.AddAsync(order);
            await orderRepository.SaveAsync();

            shoppingCartItems.ToList().ForEach(async x =>
            {
                var orderItem = new OrderItem
                {
                    BookId = x.BookId,
                    Order = order,
                    Quantity = x.Quantity
                };

                await orderItemRepository.AddAsync(orderItem);
            });

            await orderItemRepository.SaveAsync();

            shoppingCartItems.ToList().ForEach(x =>
            {
                var book = bookRepository.Get(x.BookId);

                book.Quantity -= 1;

                bookRepository.AddOrUpdate(book);

                shoppingCartItemRepository.Remove(x);
            });

            await bookRepository.SaveAsync();
            await shoppingCartItemRepository.SaveAsync();

            return order.Id;
        }

        public async Task CancelOrderAsync(int orderId, string sub)
        {
            var order = orderRepository.Get2(x => x.Id == orderId && x.Customer.Sub == sub).SingleOrDefault();

            if (order == null) return;

            order.OrderStatus = OrderStatus.Cancelled;

            orderRepository.Update(order);

            await orderRepository.SaveAsync();
        }
    }
}