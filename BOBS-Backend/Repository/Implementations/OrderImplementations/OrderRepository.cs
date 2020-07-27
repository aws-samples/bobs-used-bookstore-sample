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

namespace CustomExtensions
{
    // Extension methods must be defined in a static class.
    public static class StringExtension
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}

namespace BOBS_Backend.Repository.Implementations.OrderImplementations
{

    using CustomExtensions;
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

        public async Task<Order> CancelOrder(long id)
        {
            try
            {
                IOrderDetailRepository orderDetailRepo = new OrderDetailRepository(_context);
                IOrderStatusRepository orderStatusRepo = new OrderStatusRepository(_context);

                var orderStatus = await orderStatusRepo.FindOrderStatusById(5);

                var orderDetails = await orderDetailRepo.FindOrderDetailByOrderId(id);
                var order = await FindOrderById(id);

                using (var transaction = _context.Database.BeginTransaction())
                {
                    foreach (var detail in orderDetails)
                    {
                        if (detail.IsRemoved != true)
                        {
                            order.Subtotal -= (detail.quantity * detail.price);
                            order.Tax -= (detail.quantity * detail.price * .1);

                            await _context.SaveChangesAsync();

                            detail.Price.Quantity += detail.quantity;
                            detail.IsRemoved = true;

                            await _context.SaveChangesAsync();

                        }
                    }

                    order.OrderStatus = orderStatus;

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return order;
                }
                   
            }
            catch(DbUpdateConcurrencyException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
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

            bool Noreminder = pageNum % 10 == 0;

          
            if (start  < (totalPages / 10) || Noreminder == true)
            {
                pages = Enumerable.Range((Noreminder) ? ((start-1) * 10 + 1) : (start * 10 + 1), 10 ).ToArray();
            }
            else
            {
                pages = Enumerable.Range((Noreminder) ? ((start-1) * 10) : (start * 10 + 1), totalPages - (start * 10)).ToArray();
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


        private BinaryExpression GenerateDynamicLambdaFunctionOrderProperty(string splitFilter, ParameterExpression parameterExpression, string searchString)
        {
            var property = Expression.Property(parameterExpression, splitFilter);

            BinaryExpression lambda = null;
            MethodInfo method;
            bool isFirst = true;
            searchString = searchString.Trim();

            var table = (IQueryable)_context.GetType().GetProperty("Order").GetValue(_context, null);

            var row = Expression.Parameter(table.ElementType, "row");

            var col = Expression.Property(row, splitFilter);

            var type = col.Type.FullName;

            foreach (var subSearch in searchString.Split(' '))
            {
                try
                {

                    ConstantExpression constant = null;
                    if (type == "System.Int64")
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

            var table = (IQueryable)_context.GetType().GetProperty(splitFilter[0]).GetValue(_context, null);

            var row = Expression.Parameter(table.ElementType, "row");

            var col = Expression.Property(row, splitFilter[1]);

            var type = col.Type.FullName;
            foreach (var subSearch in searchString.Split(' '))
            {
                try
                {

                    ConstantExpression constant = null;
                    if (type == "System.Int64")
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

                    foreach (var member in splitFilter)
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

                BinaryExpression exp2 = null;

                if (!listOfFilters[i].Contains("."))
                {
                    exp2 = GenerateDynamicLambdaFunctionOrderProperty(listOfFilters[i], parameterExpression, searchString);
                }
                else
                {
                   
                    exp2 = GenerateDynamicLambdaFunctionSubOrderProperty(listOfFilters[i].Split("."), parameterExpression, searchString);



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
