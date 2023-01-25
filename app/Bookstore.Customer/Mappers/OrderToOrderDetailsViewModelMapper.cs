using Bookstore.Domain;
using Bookstore.Domain.Orders;
using Bookstore.Web.ViewModel.Orders;
using System.Linq;

namespace Bookstore.Web.Mappers
{
    public static class OrderToOrderDetailsViewModelMapper
    {
        public static OrderDetailsViewModel ToOrderDetailsViewModel(this Order order)
        {
            var viewModel = new OrderDetailsViewModel
            {
                OrderId = order.Id,
                DeliveryDate = order.DeliveryDate,
                OrderStatus = order.OrderStatus.GetDescription(),
                Total = order.Total
            };

            viewModel.OrderItems = order.OrderItems.Select(x => new OrderDetailsItemViewModel
            {
                BookId = x.BookId,
                BookName = x.Book.Name,
                ImageUrl = x.Book.FrontImageUrl,
                Price = x.Book.Price
            }).ToList();

            return viewModel;
        }
    }
}
