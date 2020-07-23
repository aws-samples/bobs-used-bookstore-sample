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

namespace BOBS_Backend.Repository.Implementations.WelcomePageImplementation
{
    public class CustomAdmin: ICustomAdminPage
    {
        private DatabaseContext _context;
        private string adminUserName;
        public CustomAdmin(DatabaseContext context)
        {
            _context = context;
            

        }

        public async Task<List<Price>> GetUpdatedBooks(IEnumerable<System.Security.Claims.Claim> claims)
        {
            // the query returns the collection of updated book models
            // Return the books updated by the current User. Returns only latest 5
            try
            {
                adminUserName = claims.FirstOrDefault(c => c.Type.Equals("cognito:username"))?.Value;
                var books = await _context.Price
                            .Where(p => p.UpdatedBy == adminUserName)
                            .Include(p => p.Book)
                            .Include(p => p.Book)
                                .ThenInclude(b => b.Genre)
                            .Include(p => p.Book)
                                .ThenInclude(b => b.Type)
                            .Include(p => p.Condition)
                            .OrderByDescending(p => p.UpdatedOn.Date)
                            .Take(4).ToListAsync();
                            
                                

                return books;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Price>> GetGlobalUpdatedBooks()
        {

            /*
             Returns the latest updates made on the inventory excluding 
             the ones make by the current user
            */
            try
            {
                var books = await _context.Price
                                .Where(p=>p.UpdatedBy != adminUserName)
                                .Include(p => p.Book)
                                .Include(p => p.Book)
                                    .ThenInclude(b => b.Genre)
                                .Include(p => p.Book)
                                    .ThenInclude(b => b.Type)
                                .Include(p => p.Condition)
                                .OrderByDescending(p => p.UpdatedOn.Date)
                                .Take(4).ToListAsync();
                return books;
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        private List<Order> FilterOrders(List<Order> allOrders)
        {
            try
            {
                List<Order> filtered_order = new List<Order>();
                DateTime todayDate = DateTime.Now;
                foreach (var order in allOrders)
                {
                    DateTime time = Convert.ToDateTime(order.DeliveryDate);
                    if (order.OrderStatus.Status == "Pending")
                    {
                        if ((time - todayDate).TotalDays <= 5)
                        {
                            filtered_order.Add(order);
                        }
                    }
                }

                return filtered_order;
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task GetImportantOrders()
        {
            var order = await _context.Order
                            .Include(o => o.Order_Id)
                            .Include(o => o.Customer)
                                .ThenInclude(c => c.Customer_Id)
                            .Include(o => o.Customer)
                                .ThenInclude(c => c.FirstName)
                            .Include(o => o.Customer)
                                .ThenInclude(c => c.LastName)
                            .Include(o => o.DeliveryDate)
                            .Include(o => o.OrderStatus)
                                .ThenInclude(os => os.Status).ToListAsync();
            var filteredOrders = FilterOrders(order);

        }

        
    }
}
