using BobsBookstore.Models.Books;
using System;
using System.Linq;
using System.Linq.Expressions;


namespace BookstoreBackend.Repository.InventoryInterface
{
    public interface IBookDatabaseCalls
    {
        IQueryable GetBaseQuery(string objPath);

        IQueryable<Price> ReturnBasePriceQuery(IQueryable query, string[] includes);


        IQueryable<Price> ReturnFilterPriceQuery(IQueryable<Price> query, Expression<Func<Price, bool>> lambda);


        IQueryable<Book> ReturnBaseBookQuery(IQueryable query, string[] includes);


        IQueryable<Book> ReturnFilterBookQuery(IQueryable<Book> query, Expression<Func<Book, bool>> lambda);


        IQueryable<Condition> ReturnBaseConditionQuery(IQueryable query, string[] includes);


        IQueryable<Condition> ReturnFilterConditionQuery(IQueryable<Condition> query, Expression<Func<Condition, bool>> lambda);

        

    }
}
