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
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using BOBS_Backend.Repository.SearchImplementations;
using System.Runtime.InteropServices;

namespace BOBS_Backend.Repository.Implementations.OrderImplementations
{

    public class OrderRepository : IOrderRepository
    {
        /*
         * Order Repository contains all functions associated with Order Model
         */

        private DatabaseContext _context;
        private readonly int _ordersPerPage = 20;
        private readonly string[] OrderIncludes = { "Customer", "Address", "OrderStatus" };
        private ISearchRepository _searchRepo;

        // Set up connection to Database 
        public OrderRepository(DatabaseContext context, ISearchRepository searchRepo)
        {
            _context = context;
            _searchRepo = searchRepo;
        }



        // Find Single Order by the Order Id
        public async Task<Order> FindOrderById(long id)
        {

            string[] pass = { "Customer", "Address", "OrderStatus" };

            var order = _context.Order
                            .Where(order => order.Order_Id == id)
                            .Include(pass)
                            .First();

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


        // Find All the Orders in the Table
        public async Task<ManageOrderViewModel> GetAllOrders(int pageNum)
        {

            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var query = (IQueryable<Order>)_searchRepo.GetBaseQuery("BOBS_Backend.Models.Order.Order");

            query = query.Include(OrderIncludes);

            var totalPages = _searchRepo.GetTotalPages(query.Count(),_ordersPerPage);

            var orders = query
                            .OrderBy(order => order.OrderStatus.OrderStatus_Id)
                            .ThenBy(order => order.DeliveryDate)
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToList();

            int[] pages = _searchRepo.GetModifiedPagesArr(pageNum, totalPages);
            


            viewModel = RetrieveViewModel("", "", pageNum, totalPages, pages, orders);

            return viewModel;
        }


        private async Task<ManageOrderViewModel> RetrieveFilterViewModel(IQueryable<Order> filterQuery, int totalPages, int pageNum, string filterValue, string searchString) 
        {
            ManageOrderViewModel viewModel = new ManageOrderViewModel();

            var orders = filterQuery
                            .OrderBy(order => order.OrderStatus.OrderStatus_Id)
                            .ThenBy(order => order.DeliveryDate)
                            .Skip((pageNum - 1) * _ordersPerPage)
                            .Take(_ordersPerPage)
                            .ToList();

            int[] pages = _searchRepo.GetModifiedPagesArr(pageNum, totalPages);

            viewModel = RetrieveViewModel(filterValue, searchString, pageNum, totalPages, pages, orders);

            return viewModel;
        }


   
        public async Task<ManageOrderViewModel> FilterList(string filterValue, string searchString, int pageNum)
        {

            ManageOrderViewModel viewModel = new ManageOrderViewModel();
            var parameterExpression = Expression.Parameter(Type.GetType("BOBS_Backend.Models.Order.Order"), "order");


            var expression = _searchRepo.ReturnExpression(parameterExpression, filterValue, searchString);

            Expression<Func<Order,bool>> lambda = Expression.Lambda<Func<Order,bool>>(expression,parameterExpression);
         
            if (lambda == null)
            {
                int[] pages = Enumerable.Range(1, 1).ToArray();


                viewModel = RetrieveViewModel("", "", 1, 1, pages, null);
                return viewModel;
            }

            var query = (IQueryable<Order>) _searchRepo.GetBaseQuery("BOBS_Backend.Models.Order.Order");

            query = query.Include(OrderIncludes);

            var filterQuery = query.Where(lambda);

            int totalPages = _searchRepo.GetTotalPages(filterQuery.Count(), _ordersPerPage);

            viewModel = await RetrieveFilterViewModel(filterQuery, totalPages, pageNum, filterValue, searchString);
            return viewModel;

        }


        public IQueryable<Order> FilterOrder(string filterValue, string searchString)
        {

            var parameterExpression = Expression.Parameter(Type.GetType("BOBS_Backend.Models.Order.Order"), "order");

            var expression = _searchRepo.ReturnExpression(parameterExpression, filterValue, searchString);

            Expression<Func<Order, bool>> lambda = Expression.Lambda<Func<Order, bool>>(expression, parameterExpression);

            var query = (IQueryable<Order>)_searchRepo.GetBaseQuery("BOBS_Backend.Models.Order.Order");

            query = query.Include(OrderIncludes);

            var filterQuery = query.Where(lambda);

            return filterQuery;
        }

    }
}
