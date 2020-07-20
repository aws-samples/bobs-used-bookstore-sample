using BOBS_Backend.Database;
using BOBS_Backend.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BOBS_Backend.Repository.OrdersInterface;
using BOBS_Backend.ViewModel.ManageOrders;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;

namespace BOBS_Backend.Repository.Implementations.OrderImplementations
{
    public class OrderRepository : IOrderRepository
    {
        /*
         * Order Repository contains all functions associated with Order Model
         */

        private DatabaseContext _context;
        private readonly int _ordersPerPage = 20;


        // Set up connection to Database 
        public OrderRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task CancelOrder(long id)
        {
            try
            {
                IOrderDetailRepository orderDetailRepo = new OrderDetailRepository(_context);

                var orderDetails = await orderDetailRepo.FindOrderDetailByOrderId(id);
                var order = await FindOrderById(id);

                foreach (var detail in orderDetails)
                {
                    if (detail.IsRemoved != true)
                    {
                        order.Refund += (detail.quantity * detail.price);
                        detail.Price.Quantity += detail.quantity;
                        detail.IsRemoved = true;

                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
           

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

        private ManageOrderViewModel RetrieveViewModel(string filterValue, string searchString, int pageNum, int totalPages, int[] pages, List<Order> order)
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

        private IQueryable<Order> GetBaseOrderQuery()
        {
            var query = _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus);
 
            return query;
        }

        private int GetTotalPages(IQueryable<Order> orderQuery)
        {
            if((orderQuery.Count() % _ordersPerPage) == 0)
            {
                return (orderQuery.Count() / _ordersPerPage);
            }
            else return (orderQuery.Count() / _ordersPerPage) + 1;
        }

        // Find All the Orders in the Table
        public async Task<ManageOrderViewModel> GetAllOrders(int pageNum)
        {

            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var totalPages = GetTotalPages(_context.Order);

            var orders = await _context.Order
                            .Include(order => order.Customer)
                            .Include(order => order.Address)
                            .Include(order => order.OrderStatus)
                            .OrderByDescending(order => order.OrderStatus.Status == "Pending")
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToListAsync();

            int[] pages = Enumerable.Range(1, totalPages).ToArray();


            viewModel = RetrieveViewModel("", "", pageNum, totalPages, pages, orders);

            return viewModel;
        }


        private async Task<ManageOrderViewModel> RetrieveFilterViewModel(IQueryable<Order> filterQuery, int totalPages, int pageNum, string filterValue, string searchString) 
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var orders = await filterQuery
                            .OrderByDescending(order => order.OrderStatus.Status == "Pending")
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToListAsync();

            int[] pages = Enumerable.Range(1, totalPages).ToArray();

            viewModel = RetrieveViewModel(filterValue, searchString, pageNum, totalPages, pages, orders);

            return viewModel;
        }


        private Expression<Func<Order,bool>> GenerateDynamicLambdaFunctionOrderProperty(string[] splitFilter, ParameterExpression parameterExpression, string searchString)
        {

            var property = Expression.Property(parameterExpression, splitFilter[2]);

            Expression<Func<Order, bool>> lambda;

            if (splitFilter[1] == "int")
            {
                try
                {
                    long value = long.Parse(searchString);

                    var constant = Expression.Constant(value);

                    var expression = Expression.Equal(property, constant);

                    lambda = Expression.Lambda<Func<Order, bool>>(expression, parameterExpression);

                 
                }
                catch
                {
                    lambda = null;
                }
            }

            else
            {
                lambda = null;
            }

            return lambda;
        }

        private Expression<Func<Order, bool>> GenerateDynamicLambdaFunctionSubOrderProperty(string[] splitFilter, ParameterExpression parameterExpression, string searchString)
        {


            Expression<Func<Order, bool>> lambda;

            if (splitFilter[1] == "int")
            {
                try
                {
                    long value = long.Parse(searchString);

                    var constant = Expression.Constant(value);

                    Expression property2 = parameterExpression;

                    foreach (var member in splitFilter[2].Split('.'))
                    {
                        property2 = Expression.PropertyOrField(property2, member);
                    }

                    var expression = Expression.Equal(property2, constant);

                    lambda = Expression.Lambda<Func<Order, bool>>(expression, parameterExpression);
                }
                catch
                {
                    lambda = null;
                }
            }

            else
            {
                var constant = Expression.Constant(searchString);

                Expression property2 = parameterExpression;

                foreach (var member in splitFilter[2].Split('.'))
                {
                    property2 = Expression.PropertyOrField(property2, member);
                }
                var method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });

                var expression = Expression.Call(property2, method, constant);

                lambda = Expression.Lambda<Func<Order, bool>>(expression, parameterExpression);
            }

            return lambda;
        }

        public async Task<ManageOrderViewModel> FilterList(string filterValue, string searchString, int pageNum)
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();
            var parameterExpression = Expression.Parameter(Type.GetType("BOBS_Backend.Models.Order.Order"), "order");


            string[] splitFilter = filterValue.Split(' ');
            Expression<Func<Order, bool>> lambda;

            if(splitFilter[0] == "Order")
            {
                lambda = GenerateDynamicLambdaFunctionOrderProperty(splitFilter, parameterExpression, searchString);
            }
            else
            {

                lambda = GenerateDynamicLambdaFunctionSubOrderProperty(splitFilter, parameterExpression, searchString);


            }

            if(lambda == null)
            {
                int[] pages = Enumerable.Range(1, 1).ToArray();


                viewModel = RetrieveViewModel("", "", 1, 1, pages, null);
                return viewModel;
            }
            
            var query = GetBaseOrderQuery();
            var filterQuery = query.Where(lambda);

            int totalPages = GetTotalPages(filterQuery);

            viewModel = await RetrieveFilterViewModel(filterQuery, totalPages, pageNum, filterValue, searchString);
            return viewModel;

        }

    }
}
