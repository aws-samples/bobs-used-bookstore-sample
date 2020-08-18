
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOBS_Backend.Repository.WelcomePageInterface;
using BOBS_Backend.ViewModel.UpdateBooks;
using Microsoft.EntityFrameworkCore;
using BOBS_Backend.Database;
using Microsoft.EntityFrameworkCore.Internal;
using BOBS_Backend.Models.Book;
using BOBS_Backend.Models.Order;
using Amazon.Runtime.Internal.Util;
using Amazon.S3.Model;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace BOBS_Backend.Repository.Implementations.WelcomePageImplementation
{
    public class CustomAdmin: ICustomAdminPage
    {
        private DatabaseContext _context;
        public CustomAdmin(DatabaseContext context)
        {
            _context = context;
            

        }

        public Task<List<Price>> GetUserUpdatedBooks(string adminUsername)
        {
            // the query returns the collection of updated book models
            // Return the books updated by the current User. Returns only latest 5
            try
            {
                if (adminUsername == null)
                    throw new ArgumentNullException("Admin username cannot be null", "adminUsername");
                var books =  _context.Price
                            .Where(p => p.UpdatedBy == adminUsername)
                            .Include(p => p.Book)
                            .Include(p => p.Book)
                                .ThenInclude(b => b.Genre)
                            .Include(p => p.Book)
                                .ThenInclude(b => b.Type)
                            .Include(p => p.Condition)
                            .Include(p => p.Book)
                                .ThenInclude(b => b.Publisher)
                            .OrderByDescending(p => p.UpdatedOn.Date)
                            .Take(Constants.TOTAL_RESULTS).ToListAsync();
                            
                                

                return books;
            }
            catch(DbException e)
            {
                throw e;
                
            }
        }
        public async Task<List<Price>> OtherUpdatedBooks(string adminUsername)
        {

            /*
             Returns the latest updates made on the inventory excluding 
             the ones make by the current user
            */
            try
            {
                
                if (adminUsername == null)
                {
                    throw new ArgumentNullException("Admin User name cannot be null", "adminUsername");
                }
                var books = await _context.Price
                                .Where(p=>p.UpdatedBy != adminUsername)
                                .Include(p => p.Book)
                                .Include(p => p.Book)
                                    .ThenInclude(b => b.Genre)
                                .Include(p => p.Book)
                                    .ThenInclude(b => b.Type)
                                .Include(p => p.Condition)
                                .Include(p=>p.Book)
                                    .ThenInclude(b=>b.Publisher)
                                .OrderByDescending(p => p.UpdatedOn.Date)
                                .Take(Constants.TOTAL_RESULTS).ToListAsync();
                
                return books;
            }catch(DbException e)
            {
                throw e;
            }
        }

        public int GetOrderSeverity(Order order, double timeDiff )
        {
            try
            {
                int severity = 0;
                long status = order.OrderStatus.OrderStatus_Id;
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
            catch(System.ArgumentNullException e)
            {
                throw new System.ArgumentNullException("Order object or timeDiff cannot be null", e);
            }
            catch(NullReferenceException e)
            {
                
                throw e;
            }
        }
        private List<FilterOrders> FilterOrders(List<Order> allOrders, int orderDayRangeMax, int orderDayRangeMin)
        {
            //filters the orders based on priority
            try {
                // list of filtered orders to be returned 
                List<FilterOrders> filtered_order = new List<FilterOrders>();
                // Date at the time 
                DateTime todayDate = DateTime.Now;
                foreach (var order in allOrders)
                {

                   
                    DateTime time = Convert.ToDateTime(order.DeliveryDate);
                    if (order.OrderStatus.Status == "Pending")
                    
                    {
                        // check pending orders which are due within 5 days
                       
                        double diff = (time - todayDate).TotalDays;
                        if ( diff <= orderDayRangeMax)
                        {
                            int severity = GetOrderSeverity(order, diff);
                            FilterOrders new_order = new FilterOrders();
                            new_order.Order = order;
                            new_order.Severity = severity;
                            filtered_order.Add(new_order);
                        }

                    }
                    // check for delayed orders. i.e today's date is past delivery date
                    else if (order.OrderStatus.Status == "En Route")
                    {
                        double diff = (todayDate - time).TotalDays;
                        if (diff > orderDayRangeMin && diff < orderDayRangeMax)
                        {
                            int severity = GetOrderSeverity(order, diff);
                            FilterOrders new_order = new FilterOrders();
                            new_order.Order = order;
                            new_order.Severity = severity;
                            filtered_order.Add(new_order);
                        }
                    }
                }
                filtered_order.OrderBy(o => o.Order.OrderStatus.OrderStatus_Id);
                return filtered_order;
            }catch(System.ArgumentNullException e)
            {

                throw new ArgumentNullException("allOrders cannot be null", e);
            }
        }
        public async Task<List<FilterOrders>> GetImportantOrders(int dateMaxRange, int dateMinRange)
        {
            /*
             Returns a filtered list of pending and EnRoute orders
             */
            try
            {
                
                var order = await _context.Order
                                .Where(o=> o.OrderStatus.OrderStatus_Id == 2 || o.OrderStatus.OrderStatus_Id == 3)
                                .Include(o => o.Customer)
                                .Include(o => o.OrderStatus)
                                  .ToListAsync();
                var filteredOrders = FilterOrders(order, dateMaxRange, dateMinRange);
                return filteredOrders;
            }
            catch (DbException e)
            {
                throw e;
            }

        }
        public List<FilterOrders> SortTable(List<FilterOrders> orders, string sortByValue)
        {
            
            switch(sortByValue){
                case "price_desc":
                    orders = orders.OrderByDescending(o => o.Order.Subtotal).ToList();
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(o => o.Order.DeliveryDate).ToList();
                    break;
                case "status_desc":
                    orders = orders.OrderByDescending(o => o.Order.OrderStatus.Status).ToList();
                    break;

                case "price":
                    orders = orders.OrderBy(o => o.Order.Subtotal).ToList();
                    break;
                case "date":
                    orders = orders.OrderBy(o => o.Order.DeliveryDate).ToList();
                    break;
                case "status":
                    orders = orders.OrderBy(o => o.Order.OrderStatus.Status).ToList();
                    break;
                default: break;
            }
            return orders;
        }
        
    }
}
