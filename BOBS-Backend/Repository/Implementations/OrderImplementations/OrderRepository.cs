using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BOBS_Backend.Repository.OrdersInterface;
using BOBS_Backend.ViewModel.ManageOrders;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BOBS_Backend.Repository.Implementations.OrderImplementations
{
    public class OrderRepository : IOrderRepository
    {
        /*
         * Order Repository contains all functions associated with Order Model
         */

        private DatabaseContext _context;
        private readonly int _ordersPerPage = 30;


        // Set up connection to Database 
        public OrderRepository(DatabaseContext context)
        {
            _context = context;
        }

        // Find Single Order by the Order Id
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


        // Find All the Orders in the Table
        public async Task<ManageOrderViewModel> GetAllOrders(int pageNum)
        {

            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var totalPages = (_context.Order.Count() / _ordersPerPage) + 1 ;

            var orders = await _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToListAsync();

            int[] pages = Enumerable.Range(1, totalPages).ToArray();

            viewModel.Orders = orders;
            viewModel.Pages = pages;
            viewModel.HasPreviousPages = (pageNum > 1);
            viewModel.CurrentPage = pageNum;
            viewModel.HasNextPages = (pageNum < totalPages);

            return viewModel;
        }

        // Returns a List of Orders filtered by the Order Id
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

        // Returns a List of Orders filtered by the Customer Id
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

        // Returns a List of Orders filtered by the Customer Username
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

        // Returns a List of Orders filtered by the Customer Email
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

        // Returns a List of Orders filtered by the Address State
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

        // With User input determines which Feature to filter by and navigates to respective function
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
