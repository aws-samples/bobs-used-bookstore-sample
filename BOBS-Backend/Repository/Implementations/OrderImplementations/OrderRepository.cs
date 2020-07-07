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

        public IQueryable<Order> GetBaseOrderQuery()
        {
            var query = _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus);
 
            return query;
        }

        public int GetTotalPages(IQueryable<Order> orderQuery)
        {
            return (orderQuery.Count() / _ordersPerPage) + 1;
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


        public async Task<ManageOrderViewModel> RetrieveFilterViewModel(IQueryable<Order> filterQuery, int totalPages, int pageNum, string filterValue, string searchString) 
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var orders = await filterQuery
                            .OrderByDescending(order => order.DeliveryDate)
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToListAsync();

            int[] pages = Enumerable.Range(1, totalPages).ToArray();

            viewModel = RetrieveViewModel(filterValue, searchString, pageNum, totalPages, pages, orders);

            return viewModel;
        }

       

        // With User input determines which Feature to filter by and navigates to respective function
        public async Task<ManageOrderViewModel> FilterList(string filterValue, string searchString,int pageNum)
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            

            switch(filterValue)
            {


                case "Order Id":
                {
                    long value = 0;

                    bool canConvert = long.TryParse(searchString, out value);

                    if (canConvert == true)
                    {
                        var query = GetBaseOrderQuery();
                        var filterQuery = query.Where(order => order.Order_Id == value);

                        int totalPages = GetTotalPages(filterQuery);

                        viewModel = await RetrieveFilterViewModel(filterQuery, totalPages, pageNum, filterValue, searchString);

                    }
                    else
                    {
                        int[] pageArr = Enumerable.Range(1, 1).ToArray();

                        viewModel = RetrieveViewModel(filterValue, searchString, 1, 1, pageArr, null);
                    }

                    break;
                }
                case "Status":
                {
                        var query = GetBaseOrderQuery();
                        var filterQuery = query.Where(order => order.OrderStatus.Status.Contains(searchString));

                        int totalPages = GetTotalPages(filterQuery);

                        viewModel = await RetrieveFilterViewModel(filterQuery, totalPages, pageNum, filterValue, searchString);
                        break;

                }
                    

                case "Customer Id":
                {
                        long value = 0;

                        bool canConvert = long.TryParse(searchString, out value);

                        if (canConvert == true)
                        {
                            var query = GetBaseOrderQuery();
                            var filterQuery = query.Where(order => order.Customer.Customer_Id == value);

                            int totalPages = GetTotalPages(filterQuery);

                            viewModel = await RetrieveFilterViewModel(filterQuery, totalPages, pageNum, filterValue, searchString);

                        }
                        else
                        {
                            int[] pageArr = Enumerable.Range(1, 1).ToArray();

                            viewModel = RetrieveViewModel(filterValue, searchString, 1, 1, pageArr, null);
                        }

                        break;
                }

                case "Username":
                {
                        var query = GetBaseOrderQuery();
                        var filterQuery = query.Where(order => order.Customer.Username.Contains(searchString));

                        int totalPages = GetTotalPages(filterQuery);

                        viewModel = await RetrieveFilterViewModel(filterQuery, totalPages, pageNum, filterValue, searchString);
                        break;
                }
  

                case "First Name":
                    {
                        var query = GetBaseOrderQuery();
                        var filterQuery = query.Where(order => order.Customer.FirstName.Contains(searchString));

                        int totalPages = GetTotalPages(filterQuery);

                        viewModel = await RetrieveFilterViewModel(filterQuery, totalPages, pageNum, filterValue, searchString);
                        break;
                    }

                case "Last Name":
                    {
                        var query = GetBaseOrderQuery();
                        var filterQuery = query.Where(order => order.Customer.LastName.Contains(searchString));

                        int totalPages = GetTotalPages(filterQuery);

                        viewModel = await RetrieveFilterViewModel(filterQuery, totalPages, pageNum, filterValue, searchString);
                        break;
                    }

                case "Email":
                    {
                        var query = GetBaseOrderQuery();
                        var filterQuery = query.Where(order => order.Customer.Email.Contains(searchString));

                        int totalPages = GetTotalPages(filterQuery);

                        viewModel = await RetrieveFilterViewModel(filterQuery, totalPages, pageNum, filterValue, searchString);
                        break;
                    }

                case "Phone":
                    {
                        var query = GetBaseOrderQuery();
                        var filterQuery = query.Where(order => order.Customer.Phone.Contains(searchString));

                        int totalPages = GetTotalPages(filterQuery);

                        viewModel = await RetrieveFilterViewModel(filterQuery, totalPages, pageNum, filterValue, searchString);
                        break;
                    }

                case "State":
                    {
                        var query = GetBaseOrderQuery();
                        var filterQuery = query.Where(order => order.Address.State.Contains(searchString));

                        int totalPages = GetTotalPages(filterQuery);

                        viewModel = await RetrieveFilterViewModel(filterQuery, totalPages, pageNum, filterValue, searchString);
                        break;
                    }

                case "Zip Code":
                    {
                        long value = 0;

                        bool canConvert = long.TryParse(searchString, out value);

                        if (canConvert == true)
                        {
                            var query = GetBaseOrderQuery();
                            var filterQuery = query.Where(order => order.Address.ZipCode == value);

                            int totalPages = GetTotalPages(filterQuery);

                            viewModel = await RetrieveFilterViewModel(filterQuery, totalPages, pageNum, filterValue, searchString);

                        }
                        else
                        {
                            int[] pageArr = Enumerable.Range(1, 1).ToArray();

                            viewModel = RetrieveViewModel(filterValue, searchString, 1, 1, pageArr, null);
                        }

                        break;
                    }

                case "City":
                    {
                        var query = GetBaseOrderQuery();
                        var filterQuery = query.Where(order => order.Address.City.Contains(searchString));

                        int totalPages = GetTotalPages(filterQuery);

                        viewModel = await RetrieveFilterViewModel(filterQuery, totalPages, pageNum, filterValue, searchString);
                        break;
                    }


                default:
                    {
                        int[] pageArr = Enumerable.Range(1, 1).ToArray();

                        viewModel = RetrieveViewModel("", searchString, 1, 1, pageArr, null);

                        break;
                    }
                    
            }

            return viewModel;

        }
    }
}
