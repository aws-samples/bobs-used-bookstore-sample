using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BOBS_Backend.Repository.Implementations
{
    public class OrderRepository : IOrderRepository
    {

        private DatabaseContext _context;

        public OrderRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Order> FindOrderById(long id)
        {
            var order = await _context.Order
                            .Where(order => order.Order_Id == id)
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .FirstAsync();

            return order;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .ToListAsync();

            return orders;
        }

        public async Task<List<Order>> FilterOrderByOrderId(string searchString)
        {
            try
            {
                long value = long.Parse(searchString);

                var orders = await _context.Order
                                .Include(order => order.Customer)
                                .Include(order => order.Address)
                                .Include(order => order.OrderStatus)
                                .Where(order => order.Order_Id == value)
                                .ToListAsync();
                return orders;
            }
            catch
            {
                List<Order> orders = null;
                return orders;
            }
        }

        public async Task<List<Order>> FilterOrderByCustomerId(string searchString)
        {
            try
            {
                long value = long.Parse(searchString);

                var orders = await _context.Order
                                .Include(order => order.Customer)
                                .Include(order => order.Address)
                                .Include(order => order.OrderStatus)
                                .Where(order => order.Customer.Customer_Id == value)
                                .ToListAsync();
                return orders;
            }
            catch
            {
                List<Order> orders = null;
                return orders;
            }
      
            
        }

        public async Task<List<Order>> FilterOrderByUsername(string searchString)
        {

            var orders =  await _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Where(order => order.Customer.Username.Contains(searchString))
                            .ToListAsync();
            return orders;
        }

        public async Task<List<Order>> FilterOrderByEmail(string searchString)
        {

            var orders = await _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Where(order => order.Customer.Email.Contains(searchString))
                            .ToListAsync();
            return orders;
        }

        public async Task<List<Order>> FilterOrderByState(string searchString)
        {

            var orders = await _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Where(order => order.Address.State.Contains(searchString))
                            .ToListAsync();
            return orders;
        }

        public async Task<List<Order>> FilterList(string filterValue, string searchString)
        {
            List<Order> orders;

            switch(filterValue)
            {
                case "Order Id":
                    orders = await FilterOrderByOrderId(searchString);
                    break;

                case "Customer Id":
                    orders = await FilterOrderByCustomerId(searchString);
                    break;

                case "Username":
                    orders = await FilterOrderByUsername(searchString);
                    break;

                case "Email":
                    orders = await FilterOrderByEmail(searchString);
                    break;

                case "State":
                    orders = await FilterOrderByState(searchString);
                    break;

                default:
                    orders = null;
                    break;
            }

            return orders;
        }
    }
}
