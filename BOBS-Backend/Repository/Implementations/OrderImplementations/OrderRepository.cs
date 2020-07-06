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

        public ManageOrderViewModel RetrieveViewModel(string filterValue, string searchString, int pageNum, int totalPages, int[] pages, List<Order> order)
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            viewModel.SearchString = searchString;
            viewModel.FilterValue = filterValue;
            viewModel.Orders = order;
            viewModel.Pages = pages;
            viewModel.HasPreviousPages = (pageNum > 1);
            viewModel.CurrentPage = pageNum;
            viewModel.HasNextPages = (pageNum < totalPages);

            return viewModel;
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
                            .OrderByDescending(order => order.DeliveryDate)
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToListAsync();

            int[] pages = Enumerable.Range(1, totalPages).ToArray();


            viewModel = RetrieveViewModel("", "", pageNum, totalPages, pages, orders);

            return viewModel;
        }

        // Returns a List of Orders filtered by the Order Id
        public async Task<ManageOrderViewModel> FilterOrderByOrderId(string searchString, int pageNum)
        {
            try
            {
                long value = long.Parse(searchString);

                ManageOrderViewModel viewModel = new ManageOrderViewModel();

                var totalPages = (_context.Order.Where(order => order.Order_Id == value).Count() / _ordersPerPage) + 1;

                var orders = await _context.Order
                                .Include(order => order.Customer)
                                .Include(order => order.Address)
                                .Include(order => order.OrderStatus)
                                .Where(order => order.Order_Id == value)
                                .OrderByDescending(order => order.DeliveryDate)
                                .Skip((pageNum - 1) * _ordersPerPage)
                                .Take(_ordersPerPage)
                                .ToListAsync();

                int[] pages = Enumerable.Range(1, totalPages).ToArray();

                viewModel = RetrieveViewModel("Order Id", searchString, pageNum, totalPages, pages, orders);

                return viewModel;
            }
            catch
            {
                ManageOrderViewModel viewModel = new ManageOrderViewModel();
                int[] pages = Enumerable.Range(1, 1).ToArray();

                viewModel = RetrieveViewModel("Order Id", searchString, 1, 1, pages, null);

                return viewModel;
            }
        }

        public async Task<ManageOrderViewModel> FilterOrderByStatus(string searchString,int pageNum)
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var totalPages = (_context.Order.Where(order => order.OrderStatus.Status.Contains(searchString)).Count() / _ordersPerPage) + 1;


            var orders = await _context.Order
                                .Include(order => order.Customer)
                                .Include(order => order.Address)
                                .Include(order => order.OrderStatus)
                                .Where(order => order.OrderStatus.Status.Contains(searchString))
                                .OrderByDescending(order => order.DeliveryDate)
                                .Skip((pageNum - 1) * _ordersPerPage)
                                .Take(_ordersPerPage)
                                .ToListAsync();

            int[] pages = Enumerable.Range(1, totalPages).ToArray();

            viewModel = RetrieveViewModel("Status", searchString, pageNum, totalPages, pages, orders);

            return viewModel;
        }

        // Returns a List of Orders filtered by the Customer Id
        public async Task<ManageOrderViewModel> FilterOrderByCustomerId(string searchString, int pageNum)
        {
            try
            {
                long value = long.Parse(searchString);

                ManageOrderViewModel viewModel = new ManageOrderViewModel();

                var totalPages = (_context.Order.Where(order => order.Customer.Customer_Id == value).Count() / _ordersPerPage) + 1;

                var orders = await _context.Order
                                .Include(order => order.Customer)
                                .Include(order => order.Address)
                                .Include(order => order.OrderStatus)
                                .Where(order => order.Customer.Customer_Id == value)
                                .OrderByDescending(order => order.DeliveryDate)
                                .Skip((pageNum - 1) * _ordersPerPage)
                                .Take(_ordersPerPage)
                                .ToListAsync();

                int[] pages = Enumerable.Range(1, totalPages).ToArray();

                viewModel = RetrieveViewModel("Customer Id", searchString, pageNum, totalPages, pages, orders);

                return viewModel;
            }
            catch
            {
                ManageOrderViewModel viewModel = new ManageOrderViewModel();
                int[] pages = Enumerable.Range(1, 1).ToArray();

                viewModel = RetrieveViewModel("Customer Id", searchString, 1, 1, pages, null);

                return viewModel;
            }
      
            
        }

        // Returns a List of Orders filtered by the Customer Username
        public async Task<ManageOrderViewModel> FilterOrderByUsername(string searchString, int pageNum)
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var totalPages = (_context.Order.Where(order => order.Customer.Username.Contains(searchString)).Count() / _ordersPerPage) + 1;

            var orders =  await _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Where(order => order.Customer.Username.Contains(searchString))
                            .OrderByDescending(order => order.DeliveryDate)
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToListAsync();


            int[] pages = Enumerable.Range(1, totalPages).ToArray();

            viewModel = RetrieveViewModel("Username", searchString, pageNum, totalPages, pages, orders);

            return viewModel;
        }

        public async Task<ManageOrderViewModel> FilterOrderByFirstName(string searchString, int pageNum)
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var totalPages = (_context.Order.Where(order => order.Customer.FirstName.Contains(searchString)).Count() / _ordersPerPage) + 1;

            var orders = await _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Where(order => order.Customer.FirstName.Contains(searchString))
                            .OrderByDescending(order => order.DeliveryDate)
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToListAsync();


            int[] pages = Enumerable.Range(1, totalPages).ToArray();

            viewModel = RetrieveViewModel("First Name", searchString, pageNum, totalPages, pages, orders);

            return viewModel;
        }

        public async Task<ManageOrderViewModel> FilterOrderByLastName(string searchString, int pageNum)
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var totalPages = (_context.Order.Where(order => order.Customer.LastName.Contains(searchString)).Count() / _ordersPerPage) + 1;

            var orders = await _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Where(order => order.Customer.LastName.Contains(searchString))
                            .OrderByDescending(order => order.DeliveryDate)
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToListAsync();


            int[] pages = Enumerable.Range(1, totalPages).ToArray();

            viewModel = RetrieveViewModel("Last Name", searchString, pageNum, totalPages, pages, orders);

            return viewModel;
        }

        // Returns a List of Orders filtered by the Customer Email
        public async Task<ManageOrderViewModel> FilterOrderByEmail(string searchString, int pageNum)
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var totalPages = (_context.Order.Where(order => order.Customer.Email.Contains(searchString)).Count() / _ordersPerPage) + 1;

            var orders = await _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Where(order => order.Customer.Email.Contains(searchString))
                            .OrderByDescending(order => order.DeliveryDate)
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToListAsync();


            int[] pages = Enumerable.Range(1, totalPages).ToArray();

            viewModel = RetrieveViewModel("Email", searchString, pageNum, totalPages, pages, orders);

            return viewModel;
        }

        public async Task<ManageOrderViewModel> FilterOrderByPhone(string searchString, int pageNum)
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var totalPages = (_context.Order.Where(order => order.Customer.Phone.Contains(searchString)).Count() / _ordersPerPage) + 1;

            var orders = await _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Where(order => order.Customer.Phone.Contains(searchString))
                            .OrderByDescending(order => order.DeliveryDate)
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToListAsync();


            int[] pages = Enumerable.Range(1, totalPages).ToArray();

            viewModel = RetrieveViewModel("Phone", searchString, pageNum, totalPages, pages, orders);

            return viewModel;
        }

        // Returns a List of Orders filtered by the Address State
        public async Task<ManageOrderViewModel> FilterOrderByState(string searchString,int pageNum)
        {

            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var totalPages = (_context.Order.Where(order => order.Address.State.Contains(searchString)).Count() / _ordersPerPage) + 1;

            var orders = await _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Where(order => order.Address.State.Contains(searchString))
                            .OrderByDescending(order => order.DeliveryDate)
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToListAsync();


            int[] pages = Enumerable.Range(1, totalPages).ToArray();

            viewModel = RetrieveViewModel("State", searchString, pageNum, totalPages, pages, orders);

            return viewModel;
        }

        public async Task<ManageOrderViewModel> FilterOrderByCity(string searchString, int pageNum)
        {

            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var totalPages = (_context.Order.Where(order => order.Address.City.Contains(searchString)).Count() / _ordersPerPage) + 1;

            var orders = await _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .Where(order => order.Address.City.Contains(searchString))
                            .OrderByDescending(order => order.DeliveryDate)
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToListAsync();


            int[] pages = Enumerable.Range(1, totalPages).ToArray();

            viewModel = RetrieveViewModel("City", searchString, pageNum, totalPages, pages, orders);

            return viewModel;
        }

        public async Task<ManageOrderViewModel> FilterOrderByZipCode(string searchString, int pageNum)
        {
            try
            {
                long value = long.Parse(searchString);

                ManageOrderViewModel viewModel = new ManageOrderViewModel();

                var totalPages = (_context.Order.Where(order => order.Address.ZipCode == value).Count() / _ordersPerPage) + 1;

                var orders = await _context.Order
                                .Include(order => order.Customer)
                                .Include(order => order.Address)
                                .Include(order => order.OrderStatus)
                                .Where(order => order.Address.ZipCode == value)
                                .OrderByDescending(order => order.DeliveryDate)
                                .Skip((pageNum - 1) * _ordersPerPage)
                                .Take(_ordersPerPage)
                                .ToListAsync();

                int[] pages = Enumerable.Range(1, totalPages).ToArray();

                viewModel = RetrieveViewModel("Zip Code", searchString, pageNum, totalPages, pages, orders);

                return viewModel;
            }
            catch
            {
                ManageOrderViewModel viewModel = new ManageOrderViewModel();
                int[] pages = Enumerable.Range(1, 1).ToArray();

                viewModel = RetrieveViewModel("Zip Code", searchString, 1, 1, pages, null);

                return viewModel;
            }


        }

        // With User input determines which Feature to filter by and navigates to respective function
        public async Task<ManageOrderViewModel> FilterList(string filterValue, string searchString,int pageNum)
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            switch(filterValue)
            {
                case "Order Id":
                    viewModel =  await FilterOrderByOrderId(searchString,pageNum);
                    break;

                case "Status":
                    viewModel = await FilterOrderByStatus(searchString, pageNum);
                    break;

                case "Customer Id":
                    viewModel = await FilterOrderByCustomerId(searchString, pageNum);
                    break;

                case "Username":
                    viewModel = await FilterOrderByUsername(searchString, pageNum);
                    break;

                case "First Name":
                    viewModel = await FilterOrderByFirstName(searchString, pageNum);
                    break;

                case "Last Name":
                    viewModel = await FilterOrderByLastName(searchString, pageNum);
                    break;

                case "Email":
                    viewModel = await FilterOrderByEmail(searchString, pageNum);
                    break;

                case "Phone":
                    viewModel = await FilterOrderByPhone(searchString, pageNum);
                    break;

                case "State":
                    viewModel = await FilterOrderByState(searchString, pageNum);
                    break;

                case "Zip Code":
                    viewModel = await FilterOrderByZipCode(searchString, pageNum);
                    break;

                case "City":
                    viewModel = await FilterOrderByCity(searchString, pageNum);
                    break;
                                        

                default:
                    int[] pages = Enumerable.Range(1, 1).ToArray();

                    viewModel.Orders = null;
                    viewModel.FilterValue = filterValue;
                    viewModel.SearchString = searchString;
                    viewModel.Pages = pages;
                    viewModel.HasPreviousPages = false;
                    viewModel.CurrentPage = 1;
                    viewModel.HasNextPages = false;
                    break;
            }

            return viewModel;

        }
    }
}
