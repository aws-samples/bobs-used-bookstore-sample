using Amazon.Runtime;
using BOBS_Backend.Database;
using BOBS_Backend.Models.Book;
using BOBS_Backend.Repository.Implementations.SearchImplementation;
using BOBS_Backend.Repository.InventoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository.Implementations.InventoryImplementation
{
    public class BookDatabaseCalls : IBookDatabaseCalls
    {


        private DatabaseContext _context;
        private ExpressionFunction _expFunc;
        public BookDatabaseCalls(DatabaseContext context)
        {
            _context = context;
        }

     
        public IQueryable GetBaseQuery(string objPath)
        {
            var query = _context.Query(objPath);

            return query;

        }

        public IQueryable<Price> ReturnBasePriceQuery(IQueryable query, string[] includes) 
        {
            var result = (IQueryable<Price>)query;
            result = result.Include(includes);

            return result;
        }


        public IQueryable<Price> ReturnFilterPriceQuery(IQueryable<Price> query, Expression<Func<Price, bool>> lambda) 
        {
            return query.Where(lambda);

        }


        public IQueryable<Book> ReturnBaseBookQuery(IQueryable query, string[] includes) 
        {
            var result = (IQueryable<Book>)query;
            result = result.Include(includes);

            return result;

        }


        public IQueryable<Book> ReturnFilterBookQuery(IQueryable<Book> query, Expression<Func<Book, bool>> lambda)
        {
            return query.Where(lambda);


        }


        public IQueryable<Condition> ReturnBaseConditionQuery(IQueryable query, string[] includes) 
        {
            var result = (IQueryable<Condition>)query;
            result = result.Include(includes);

            return result;

        }


       

        

        public IQueryable<Condition> ReturnFilterConditionQuery(IQueryable<Condition> query, Expression<Func<Condition, bool>> lambda)
        {
            return query.Where(lambda);

        }

        
    }
}
