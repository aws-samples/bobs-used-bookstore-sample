using Bookstore.Customer.ViewModel.Checkout;
using Bookstore.Domain.Orders;
using System.Linq;

namespace Bookstore.Customer.Mappers
{
    public static class OrderToCheckoutFinishedViewModel
    {
        public static CheckoutFinishedViewModel ToCheckoutFinishedViewModel(this Order order)
        {
            return new CheckoutFinishedViewModel
            {
                Items = order.OrderItems.Select(x => new CheckoutFinishedItemViewModel
                {
                    BookId = x.Book.Id,
                    Bookname = x.Book.Name,
                    Price = x.Book.Price,
                    Quantity = x.Quantity,
                    Url = x.Book.FrontImageUrl
                })
            };
        }
    }
}
