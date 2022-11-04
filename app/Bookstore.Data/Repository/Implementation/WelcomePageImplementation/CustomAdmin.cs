using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Data.Data;
using Bookstore.Data.Repository.Interface.WelcomePageInterface;
using Bookstore.Domain.Books;
using Bookstore.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data.Repository.Implementation.WelcomePageImplementation
{
    public class CustomAdmin : ICustomAdminPage
    {
        private readonly ApplicationDbContext _context;

        public CustomAdmin(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Price>> GetUserUpdatedBooksAsync(string adminUsername)
        {
            // the query returns the collection of updated book models
            // Return the books updated by the current User. Returns only latest 5
            if (adminUsername == null)
                throw new ArgumentNullException("Admin username cannot be null", "adminUsername");

            var books = _context.Price
                .Where(p => p.UpdatedBy == adminUsername)
                .Include(p => p.Book)
                .Include(p => p.Book)
                .ThenInclude(b => b.Genre)
                .Include(p => p.Book)
                .ThenInclude(b => b.BookType)
                .Include(p => p.Condition)
                .Include(p => p.Book)
                .ThenInclude(b => b.Publisher)
                .OrderByDescending(p => p.UpdatedOn.Date)
                .Take(Constants.TotalResults).ToListAsync();

            return books;
        }

        public async Task<List<Price>> OtherUpdatedBooksAsync(string adminUsername)
        {
            /*
             Returns the latest updates made on the inventory excluding 
             the ones make by the current user
            */
            if (adminUsername == null)
                throw new ArgumentNullException("Admin User name cannot be null", "adminUsername");

            var books = await _context.Price
                .Where(p => p.UpdatedBy != adminUsername)
                .Include(p => p.Book)
                .Include(p => p.Book)
                .ThenInclude(b => b.Genre)
                .Include(p => p.Book)
                .ThenInclude(b => b.BookType)
                .Include(p => p.Condition)
                .Include(p => p.Book)
                .ThenInclude(b => b.Publisher)
                .OrderByDescending(p => p.UpdatedOn.Date)
                .Take(Constants.TotalResults).ToListAsync();

            return books;
        }

        //public async Task<List<FilterOrdersDto>> GetImportantOrdersAsync(int dateMaxRange, int dateMinRange)
        //{
        //    /*
        //     Returns a filtered list of pending and EnRoute orders
        //     */
        //    var order = await _context.Order
        //        .Where(o => o.OrderStatus.OrderStatus_Id == 2 || o.OrderStatus.OrderStatus_Id == 3)
        //        .Include(o => o.Customer)
        //        .Include(o => o.OrderStatus)
        //        .ToListAsync();

        //    var filteredOrders = FilterOrders(order, dateMaxRange, dateMinRange);
        //    return filteredOrders;
        //}

        //public List<FilterOrdersDto> SortTable(List<FilterOrdersDto> orders, string sortByValue)
        //{
        //    switch (sortByValue)
        //    {
        //        case "price_desc":
        //            orders = orders.OrderByDescending(o => o.Order.Subtotal).ToList();
        //            break;
        //        case "date_desc":
        //            orders = orders.OrderByDescending(o => o.Order.DeliveryDate).ToList();
        //            break;
        //        case "status_desc":
        //            orders = orders.OrderByDescending(o => o.Order.OrderStatus.Status).ToList();
        //            break;

        //        case "OrderDetailPrice":
        //            orders = orders.OrderBy(o => o.Order.Subtotal).ToList();
        //            break;
        //        case "date":
        //            orders = orders.OrderBy(o => o.Order.DeliveryDate).ToList();
        //            break;
        //        case "status":
        //            orders = orders.OrderBy(o => o.Order.OrderStatus.Status).ToList();
        //            break;
        //    }

        //    return orders;
        //}

        public int GetOrderSeverity(Order order, double timeDiff)
        {
            try
            {
                var severity = 0;
                var status = order.OrderStatus.OrderStatus_Id;
                switch (status)
                {
                    case 2:
                        if (timeDiff <= 0)
                            severity = 2;
                        else
                            severity = 1;
                        break;
                    case 3:
                        severity = 2;
                        break;
                }

                return severity;
            }
            catch (ArgumentNullException e)
            {
                throw new ArgumentNullException("Order object or timeDiff cannot be null", e);
            }
        }

        //private List<FilterOrdersDto> FilterOrders(List<Order> allOrders, int orderDayRangeMax, int orderDayRangeMin)
        //{
        //    //filters the orders based on priority
        //    try
        //    {
        //        // list of filtered orders to be returned 
        //        var filtered_order = new List<FilterOrdersDto>();
        //        // Date at the time 
        //        var todayDate = DateTime.Now.ToUniversalTime();

        //        foreach (var order in allOrders)
        //        {
        //            var time = DateTime.ParseExact(order.DeliveryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToUniversalTime();
        //            if (order.OrderStatus.Status == "Pending")
        //            {
        //                // check pending orders which are due within 5 days
        //                var diff = (time - todayDate).TotalDays;
        //                if (diff <= orderDayRangeMax)
        //                {
        //                    var severity = GetOrderSeverity(order, diff);
        //                    var new_order = new FilterOrdersDto();
        //                    new_order.Order = order;
        //                    new_order.Severity = severity;
        //                    filtered_order.Add(new_order);
        //                }
        //            }
        //            // check for delayed orders. i.e today's date is past delivery date
        //            else if (order.OrderStatus.Status == "En Route")
        //            {
        //                var diff = (todayDate - time).TotalDays;
        //                if (diff > orderDayRangeMin && diff < orderDayRangeMax)
        //                {
        //                    var severity = GetOrderSeverity(order, diff);
        //                    var new_order = new FilterOrdersDto();
        //                    new_order.Order = order;
        //                    new_order.Severity = severity;
        //                    filtered_order.Add(new_order);
        //                }
        //            }
        //        }

        //        filtered_order.OrderBy(o => o.Order.OrderStatus.OrderStatus_Id);
        //        return filtered_order;
        //    }
        //    catch (ArgumentNullException e)
        //    {
        //        throw new ArgumentNullException("allOrders cannot be null", e);
        //    }
        //}
    }
}