using Bookstore.Domain.Carts;
using Bookstore.Domain.Customers;

namespace Bookstore.Domain.Orders
{
    public interface IOrderService
    {
        Task<IPaginatedList<Order>> GetOrdersAsync(OrderFilters filters, int pageIndex = 1, int pageSize = 10);

        Task<IEnumerable<Order>> GetOrdersAsync(string sub);

        Task<Order> GetOrderAsync(int id);

        Task UpdateOrderStatusAsync(UpdateOrderStatusDto updateOrderStatusDto);

        Task<int> CreateOrderAsync(string shoppingCartId, string sub, int selectedAddressId);

        Task CancelOrderAsync(int id, string sub);

        Task<OrderStatistics> GetStatisticsAsync();
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly ICustomerRepository customerRepository;

        public OrderService(IOrderRepository orderRepository,
            IShoppingCartRepository shoppingCartRepository,
            ICustomerRepository customerRepository)
        {
            this.orderRepository = orderRepository;
            this.shoppingCartRepository = shoppingCartRepository;
            this.customerRepository = customerRepository;
        }

        public async Task<IPaginatedList<Order>> GetOrdersAsync(OrderFilters filters, int pageIndex = 1, int pageSize = 10)
        {
            return await orderRepository.ListAsync(filters, pageIndex, pageSize);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(string sub)
        {
            return await orderRepository.ListAsync(sub);
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await orderRepository.GetAsync(id);
        }

        public async Task UpdateOrderStatusAsync(UpdateOrderStatusDto dto)
        {
            var order = await orderRepository.GetAsync(dto.OrderId);

            order.OrderStatus = dto.OrderStatus;

            order.UpdatedOn = DateTime.UtcNow;

            await orderRepository.SaveChangesAsync();
        }

        public async Task<int> CreateOrderAsync(string shoppingCartId, string sub, int selectedAddressId)
        {
            var shoppingCart = await shoppingCartRepository.GetAsync(shoppingCartId);
            var customer = await customerRepository.GetAsync(sub);

            var order = new Order
            {
                AddressId = selectedAddressId,
                Customer = customer,
                OrderStatus = OrderStatus.Pending,
                CreatedBy = sub
            };

            await orderRepository.AddAsync(order);

            shoppingCart.GetShoppingCartItems(ShoppingCartItemFilter.ExcludeOutOfStockItems).ToList().ForEach(x =>
            {
                order.AddOrderItem(x.Book, x.Quantity);

                x.Book.ReduceStockLevel(x.Quantity);

                shoppingCart.RemoveShoppingCartItemById(x.Id);
            });

            // Because each repository implements a unit of work, changes to the shopping cart and to stock levels 
            // are captured by the unit of work and can be persisted by called SaveChangesAsync on _any_ repository.
            await orderRepository.SaveChangesAsync();

            return order.Id;
        }

        public async Task CancelOrderAsync(int id, string sub)
        {
            var order = await orderRepository.GetAsync(id, sub);

            if (order == null) return;

            order.OrderStatus = OrderStatus.Cancelled;

            await orderRepository.SaveChangesAsync();
        }

        public async Task<OrderStatistics> GetStatisticsAsync()
        {
            return await orderRepository.GetStatisticsAsync();
        }

        
    }
}