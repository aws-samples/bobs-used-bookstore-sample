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

namespace BOBS_Backend.Repository.Implementations.WelcomePageImplementation
{
    public class CustomAdmin: ICustomAdminPage
    {
        private DatabaseContext _context;
        public CustomAdmin(DatabaseContext context)
        {
            _context = context;
            

        }

        public async Task<List<Price>> GetUpdatedBooks(string adminUsername)
        {
            // the query returns the collection of updated book models
            // Return the books updated by the current User. Returns only latest 5
            try
            {
                
                var books = await _context.Price
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
                            .Take(8).ToListAsync();
                            
                                

                return books;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Price>> GetGlobalUpdatedBooks(string adminUsername)
        {

            /*
             Returns the latest updates made on the inventory excluding 
             the ones make by the current user
            */
            try
            {
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
                                .Take(8).ToListAsync();
                return books;
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        private List<Order> FilterOrders(List<Order> allOrders)
        {
            //filters the orders based on priority
            try
            {
                // list of filtered orders to be returned 
                List<Order> filtered_order = new List<Order>();
                // Date at the time 
                DateTime todayDate = DateTime.Now;
                foreach (var order in allOrders)
                {

                   
                    DateTime time = Convert.ToDateTime(order.DeliveryDate);
                    if (order.OrderStatus.OrderStatus_Id == 2)
                    
                    {
                        // check pending orders which are due within 5 days
                       
                        double diff = (time - todayDate).TotalDays;
                        if ( diff <= 5)
                        {
                            filtered_order.Add(order);
                        }

                    }
                    // check for delayed orders. i.e today's date is past delivery date
                    else if (order.OrderStatus.OrderStatus_Id == 3)
                    {
                        double diff = (todayDate - time).TotalDays;
                        if (diff > 0 && diff < 5)
                        {
                            filtered_order.Add(order);
                        }
                    }
                }
                filtered_order.OrderBy(o => o.OrderStatus.OrderStatus_Id);
                return filtered_order;
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Order>> GetImportantOrders()
        {
            try
            {
                
                var order = await _context.Order
                                .Where(o=> o.OrderStatus.OrderStatus_Id == 2 || o.OrderStatus.OrderStatus_Id == 3)
                                .Include(o => o.Customer)
                                .Include(o => o.OrderStatus)
                                   
                                  .ToListAsync();
                var filteredOrders = FilterOrders(order);
                return filteredOrders;
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }
        public List<Order> SortTable(List<Order> orders, string sortByValue)
        {
            
            switch(sortByValue){
                case "price_desc":
                    orders = orders.OrderByDescending(o => o.Subtotal).ToList();
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(o => o.DeliveryDate).ToList();
                    break;
                case "price":
                    orders = orders.OrderBy(o => o.Subtotal).ToList();
                    break;
                case "date":
                    orders = orders.OrderBy(o => o.DeliveryDate).ToList();
                    break;
                default: break;
            }
            return orders;
        }
        
    }
}
