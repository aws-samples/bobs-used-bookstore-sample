using Bookstore.Admin.ViewModel.Orders;
using Bookstore.Domain.Orders;

namespace Bookstore.Admin.Mappers.Orders
{
    public static class OrderToOrderDetailsViewModelMapper
    {
        public static OrderDetailsViewModel ToOrderDetailsViewModel(this Order order)
        {
            var result = new OrderDetailsViewModel
            {
                OrderId = order.Id,
                CustomerName = order.Customer.FullName,
                SelectedOrderStatus = order.OrderStatus,
                AddressLine1 = order.Address.AddressLine1,
                AddressLine2 = order.Address.AddressLine2,
                City = order.Address.City,
                State = order.Address.State,
                ZipCode = order.Address.ZipCode,
                Country = order.Address.Country,
                Subtotal = order.Subtotal,
                Tax = order.Tax,
                OrderDate = order.CreatedOn,
                DeliveryDate = order.DeliveryDate                  
            };

            return result;
        }
    }
}