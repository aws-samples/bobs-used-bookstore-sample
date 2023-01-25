using Bookstore.Domain.Orders;
using Bookstore.Web.Areas.Admin.Models.Orders;
using System.Collections.Generic;

namespace Bookstore.Web.Areas.Admin.Mappers.Orders
{
    public static class OrderDetailsListToOrderDetailsViewModelMapper
    {
        public static OrderDetailsViewModel AddOrderDetails(this OrderDetailsViewModel model, IEnumerable<OrderItem> orderDetails)
        {
            foreach (var orderItem in orderDetails)
            {
                model.Items.Add(new OrderDetailsItemViewModel
                {
                    Author = orderItem.Book.Author,
                    BookType = orderItem.Book.BookType.Text,
                    Condition = orderItem.Book.Condition.Text,
                    Genre = orderItem.Book.Genre.Text,
                    Name = orderItem.Book.Name,
                    Price = orderItem.Book.Price,
                    Publisher = orderItem.Book.Publisher.Text
                });
            }

            return model;
        }
    }
}