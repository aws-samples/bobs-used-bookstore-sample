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

        public Order FindOrderById(long id)
        {
            var order = _context.Order
                            .Where(order => order.Order_Id == id)
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .First();

            return order;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            var orders = _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .ToList();

            return orders;
        }

        public IEnumerable<Order> FilterOrderByOrderId(string searchString)
        {
            try
            {
                long value = long.Parse(searchString);

                var orders = _context.Order
                                .Include(order => order.Customer)
                                .Include(order => order.Address)
                                .Include(order => order.OrderStatus)
                                .Where(order => order.Order_Id == value)
                                .ToList();
                return orders;
            }
            catch
            {
                IEnumerable<Order> orders = null;
                return orders;
            }
        }

        public IEnumerable<Order> FilterOrderByCustomerId(string searchString)
        {
            try
            {
                long value = long.Parse(searchString);

                var orders = _context.Order
                                .Include(order => order.Customer)
                                .Include(order => order.Address)
                                .Include(order => order.OrderStatus)
                                .Where(order => order.Customer.Customer_Id == value)
                                .ToList();
                return orders;
            }
            catch
            {
                IEnumerable<Order> orders = null;
                return orders;
            }
      
            
        }

        public IEnumerable<Order> FilterOrderByUsername(string searchString)
        {

            var orders = _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Where(order => order.Customer.Username.Contains(searchString))
                            .ToList();
            return orders;
        }

        public IEnumerable<Order> FilterOrderByEmail(string searchString)
        {

            var orders = _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Where(order => order.Customer.Email.Contains(searchString))
                            .ToList();
            return orders;
        }

        public IEnumerable<Order> FilterOrderByState(string searchString)
        {

            var orders = _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Where(order => order.Address.State.Contains(searchString))
                            .ToList();
            return orders;
        }

        public IEnumerable<Order> FilterList(string filterValue, string searchString)
        {
            IEnumerable<Order> orders;

            switch(filterValue)
            {
                case "Order Id":
                    orders = FilterOrderByOrderId(searchString);
                    break;

                case "Customer Id":
                    orders = FilterOrderByCustomerId(searchString);
                    break;

                case "Username":
                    orders = FilterOrderByUsername(searchString);
                    break;

                case "Email":
                    orders = FilterOrderByEmail(searchString);
                    break;

                case "State":
                    orders = FilterOrderByState(searchString);
                    break;

                default:
                    orders = null;
                    break;
            }

            return orders;
        }
    }
}
