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
                            .Take(5).ToListAsync();
                            
                                

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
            try
            {
                var books = await _context.Price
                                .Include(p => p.Book)
                                .Include(p => p.Book)
                                    .ThenInclude(b => b.Genre)
                                .Include(p => p.Book)
                                    .ThenInclude(b => b.Type)
                                .Include(p => p.Condition)
                                .OrderByDescending(p => p.UpdatedOn.Date)
                                .Take(5).ToListAsync();
                return books;
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
