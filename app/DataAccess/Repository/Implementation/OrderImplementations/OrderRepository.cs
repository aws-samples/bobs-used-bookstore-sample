using System.Collections.Generic;
using System.Linq;
using DataAccess.Dtos;
using DataAccess.Repository.Interface.OrdersInterface;
using DataAccess.Repository.Interface.SearchImplementations;
using DataModels.Orders;

namespace DataAccess.Repository.Implementation.OrderImplementations
{
    // Order Repository contains all functions associated with Order Model
    public class OrderRepository : IOrderRepository
    {
        private readonly int _ordersPerPage = 20;
        private readonly string[] OrderIncludes = { "Customer", "Address", "OrderStatus" };
        private readonly IExpressionFunction _expFunc;
        private readonly IOrderDatabaseCalls _orderDbCalls;
        private readonly ISearchRepository _searchRepo;

        public OrderRepository(ISearchRepository searchRepo, IOrderDatabaseCalls orderDbCalls,
            IExpressionFunction expFunc)
        {
            _searchRepo = searchRepo;
            _orderDbCalls = orderDbCalls;
            _expFunc = expFunc;
        }

        // Find single order by the Order Id
        public Order FindOrderById(long id)
        {
            var filterValue = "Order_Id";
            var searchString = "" + id;
            var inBetween = "";
            var operand = "==";
            var negate = "false";

            Order order = null;
            try
            {
                var query = FilterOrder(filterValue, searchString, inBetween, operand, negate);

                order = query.First();
            }
            catch
            {
                order = null;
            }

            return order;
        }

        // Find all the orders in the table
        public ManageOrderDto GetAllOrders(int pageNum)
        {
            var viewModel = new ManageOrderDto();

            var orderBase = _orderDbCalls.GetBaseQuery("DataModels.Orders.Order");

            var query = _orderDbCalls.ReturnBaseQuery<Order>(orderBase, OrderIncludes);

            var totalPages = _searchRepo.GetTotalPages(query.Count(), _ordersPerPage);

            viewModel = RetrieveFilterDto(query, totalPages, pageNum, "", "");

            return viewModel;
        }

        public ManageOrderDto FilterList(string filterValue, string searchString, int pageNum)
        {
            var parameterExpression = _expFunc.ReturnParameterExpression(typeof(Order), "Order");

            var expression = _searchRepo.ReturnExpression(parameterExpression, filterValue, searchString);

            var lambda = _expFunc.ReturnLambdaExpression<Order>(expression, parameterExpression);

            if (lambda == null)
            {
                var pages = Enumerable.Range(1, 1).ToArray();

                return RetrieveDto("", "", 1, 1, pages, null);
            }

            var orderBase = _orderDbCalls.GetBaseQuery("DataModels.Orders.Order");

            var query = _orderDbCalls.ReturnBaseQuery<Order>(orderBase, OrderIncludes);

            var filterQuery = _orderDbCalls.ReturnFilterQuery(query, lambda);

            var totalPages = _searchRepo.GetTotalPages(filterQuery.Count(), _ordersPerPage);

            var manageOrderDto = RetrieveFilterDto(filterQuery, totalPages, pageNum, filterValue, searchString);
            return manageOrderDto;
        }

        public IQueryable<Order> FilterOrder(string filterValue, string searchString, string inBetween, string operand,
            string negate)
        {
            var tableName = "Order";

            var lambda =
                _expFunc.ReturnLambdaExpression<Order>(tableName, filterValue, searchString, inBetween, operand,
                    negate);

            var orderBase = _orderDbCalls.GetBaseQuery("DataModels.Orders.Order");

            var query = _orderDbCalls.ReturnBaseQuery<Order>(orderBase, OrderIncludes);

            var filterQuery = _orderDbCalls.ReturnFilterQuery(query, lambda);

            return filterQuery;
        }

        private ManageOrderDto RetrieveDto(string filterValue, string searchString, int pageNum, int totalPages,
            int[] pages, List<Order> order)
        {
            var manageOrderDto = new ManageOrderDto
            {
                SearchString = searchString,
                FilterValue = filterValue,
                Orders = order,
                Pages = pages,
                HasPreviousPages = pageNum > 1,
                CurrentPage = pageNum,
                HasNextPages = pageNum < totalPages
            };

            return manageOrderDto;
        }

        private ManageOrderDto RetrieveFilterDto(IQueryable<Order> filterQuery, int totalPages, int pageNum,
            string filterValue, string searchString)
        {
            var orders = filterQuery
                .OrderBy(order => order.OrderStatus.Position)
                .ThenBy(order => order.DeliveryDate)
                .Skip((pageNum - 1) * _ordersPerPage)
                .Take(_ordersPerPage)
                .ToList();

            var pages = _searchRepo.GetModifiedPagesArr(pageNum, totalPages);

            var manageOrderDto = RetrieveDto(filterValue, searchString, pageNum, totalPages, pages, orders);

            return manageOrderDto;
        }
    }
}