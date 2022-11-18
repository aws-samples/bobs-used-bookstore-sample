using Bookstore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bookstore.Data.Repository.Interface
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        TModel Get(int id);

        TModel Get(long? id);

        TModel Get(string id);

        IEnumerable<TModel> GetAll(string includeProperties = "");

        void Add(TModel entity);

        void Remove(TModel entity);

        void Update(TModel entity);

        public IEnumerable<TModel> Get(
            Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            string includeProperties = "");

        public IEnumerable<TModel> Get2(Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            params Expression<Func<TModel, object>>[] includeProperties);

        void Save();
        Task SaveAsync();
        Task AddAsync(TModel entity);
        void AddOrUpdate(TModel entity);
        PaginatedList<TModel> GetPaginated(Expression<Func<TModel, bool>> filter = null, Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null, int pageIndex = 1, int pageSize = 10, params Expression<Func<TModel, object>>[] includeProperties);
    }
}