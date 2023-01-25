using Bookstore.Domain;
using Bookstore.Domain.Orders;
using Bookstore.Web.ViewModel.Orders;
using System.Collections.Generic;
using System.Linq;

namespace Bookstore.Web.Mappers
{
    public static class OrdersToOrderIndexViewModelMapper
    {
        public static OrderIndexViewModel ToOrderIndexViewModel(this IEnumerable<Order> orders)
        {
            var viewModel = new OrderIndexViewModel
            {
                OrderItems = orders.Select(x => new OrderIndexItemViewModel
                {
                    Id = x.Id,
                    DeliveryDate = x.DeliveryDate,
                    OrderStatus = x.OrderStatus.GetDescription(),
                    SubTotal = x.SubTotal
                }).ToList()
            };

            return viewModel;
        }
    }
}