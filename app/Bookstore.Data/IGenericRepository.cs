using Bookstore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bookstore.Data
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        TModel Get(int id);

        IEnumerable<TModel> GetAll(string includeProperties = "");

        void Remove(TModel entity);

        void Update(TModel entity);

        public IEnumerable<TModel> Get(Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            params Expression<Func<TModel, object>>[] includeProperties);

        Task SaveAsync();

        Task AddAsync(TModel entity);

        void AddOrUpdate(TModel entity);

        PaginatedList<TModel> GetPaginated(Expression<Func<TModel, bool>> filter = null, Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null, int pageIndex = 1, int pageSize = 10, params Expression<Func<TModel, object>>[] includeProperties);

        PaginatedList<TModel> GetPaginated(List<Expression<Func<TModel, bool>>> filters, Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null, int pageIndex = 1, int pageSize = 10, params Expression<Func<TModel, object>>[] includeProperties);
    }
}