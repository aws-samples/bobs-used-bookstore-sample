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
using Amazon.Rekognition.Model;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                        order.Subtotal -= (detail.quantity * detail.price);
                        order.Tax -= (detail.quantity * detail.price * .1); 
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

        private int[] GetModifiedPagesArr(int pageNum, int totalPages)
        {
            int[] pages = null;

            var start = pageNum / 10;

            if ((start * 10) + 10 < totalPages)
            {
                pages = Enumerable.Range(start * 10 + 1, 10).ToArray();
            }
            else
            {
                pages = Enumerable.Range(start * 10 + 1, totalPages - (start * 10)).ToArray();
            }

            return pages;
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

            int[] pages = GetModifiedPagesArr(pageNum, totalPages);
            


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

            int[] pages = GetModifiedPagesArr(pageNum, totalPages);

            viewModel = RetrieveViewModel(filterValue, searchString, pageNum, totalPages, pages, orders);

            return viewModel;
        }


        private BinaryExpression GenerateDynamicLambdaFunctionOrderProperty(string[] splitFilter, ParameterExpression parameterExpression, string searchString)
        {
            var property = Expression.Property(parameterExpression, splitFilter[2]);

            BinaryExpression lambda = null;
            MethodInfo method;
            bool isFirst = true;
            searchString = searchString.Trim();

            foreach(var subSearch in searchString.Split(' '))
            {
                try
                {
                    ConstantExpression constant = null;
                    if (splitFilter[1] == "int")
                    {
                        long value = 0;

                        bool res = long.TryParse(subSearch, out value);

                        constant = Expression.Constant(value);
                        method = typeof(long).GetMethod("Equals", new Type[] { typeof(int) });
                    }
                    else
                    {
                        constant = Expression.Constant(subSearch);
                        method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
                    }

                    var expression = Expression.Call(property, method, constant);


                    if (isFirst)
                    {
                        lambda = Expression.Or(expression, expression);
                        isFirst = false;


                    }
                    else
                    {
                        lambda = Expression.Or(lambda, expression);
                        isFirst = false;
                    }


                }
                catch
                {
                }
            }

            return lambda;

        }




        private BinaryExpression GenerateDynamicLambdaFunctionSubOrderProperty(string[] splitFilter, ParameterExpression parameterExpression, string searchString)
        {
            BinaryExpression lambda = null;
            MethodInfo method;
            bool isFirst = true;
            searchString = searchString.Trim();
            foreach(var subSearch in searchString.Split(' '))
            {
                try
                {
                    ConstantExpression constant = null;
                    if (splitFilter[1] == "int")
                    {
                        long value = 0;

                        bool res = long.TryParse(subSearch, out value);

                        constant = Expression.Constant(value);
                        method = typeof(long).GetMethod("Equals", new Type[] { typeof(int) });
                    }
                    else
                    {
                        constant = Expression.Constant(subSearch);
                        method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
                    }

                    Expression property2 = parameterExpression;

                    foreach (var member in splitFilter[2].Split('.'))
                    {
                        property2 = Expression.PropertyOrField(property2, member);
                    }

                    var expression = Expression.Call(property2, method, constant);

                    if (isFirst)
                    {
                        lambda = Expression.Or(expression, expression);
                        isFirst = false;


                    }
                    else
                    {
                        lambda = Expression.Or(lambda, expression);
                        isFirst = false;
                    }

                }
                catch
                {
                }
            }
            

            return lambda;

        }



        public async Task<ManageOrderViewModel> FilterList(string filterValue, string searchString, int pageNum)
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();
            var parameterExpression = Expression.Parameter(Type.GetType("BOBS_Backend.Models.Order.Order"), "order");


            string[] listOfFilters = filterValue.Split(' ');
            bool isFirst = true;
            BinaryExpression expression = null;


            for (int i = 1; i < listOfFilters.Length; i++)
            {
                string[] splitFilter = listOfFilters[i].Split('-');

                BinaryExpression exp2 = null;

                if (splitFilter[0] == "Order")
                {
                    exp2 = GenerateDynamicLambdaFunctionOrderProperty(splitFilter, parameterExpression, searchString);
                }
                else
                {
                   
                    exp2 = GenerateDynamicLambdaFunctionSubOrderProperty(splitFilter, parameterExpression, searchString);



                }

                if(exp2 == null)
                {
                    continue;
                }
                if (isFirst )
                {
                    expression = Expression.And(exp2, exp2);
                    isFirst = false;


                }
                else
                {
                    expression = Expression.And(expression, exp2);
                    isFirst = false;
                }

                

            }

            Expression<Func<Order,bool>> lambda = Expression.Lambda<Func<Order,bool>>(expression,parameterExpression);

            if (lambda == null)
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
