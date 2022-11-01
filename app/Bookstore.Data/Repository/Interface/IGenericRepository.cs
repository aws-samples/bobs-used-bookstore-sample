using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
    }
}