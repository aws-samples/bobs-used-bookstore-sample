using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bookstore.Data.Data;
using Bookstore.Data.Repository.Interface;
using Bookstore.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data.Repository.Implementation
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        protected DbContext context;
        protected DbSet<TModel> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<TModel>();
        }

        public void Add(TModel entity)
        {
            context.Set<TModel>().Add(entity);
        }

        public async Task AddAsync(TModel entity)
        {
            await context.Set<TModel>().AddAsync(entity);
        }

        public void AddOrUpdate(TModel entity)
        {
             context.Set<TModel>().Update(entity);
        }

        public TModel Get(int id)
        {
            return context.Set<TModel>().Find(id);
        }

        public TModel Get(long? id)
        {
            return context.Set<TModel>().Find(id);
        }

        public TModel Get(string id)
        {
            return context.Set<TModel>().Find(id);
        }

        public IEnumerable<TModel> GetAll(string includeProperties = "")
        {
            IQueryable<TModel> query = context.Set<TModel>();

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }

        public void Remove(TModel entity)
        {
            context.Set<TModel>().Remove(entity);
        }

        public void Update(TModel entity)
        {
            context.Update(entity);
            //context.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<TModel> Get(Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TModel> query = dbSet;

            if (filter != null) query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public IEnumerable<TModel> Get2(Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            params Expression<Func<TModel, object>>[] includeProperties)
        {
            IQueryable<TModel> query = dbSet;

            if (filter != null) query = query.Where(filter);

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public PaginatedList<TModel> GetPaginated(Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null, int pageIndex = 1, int pageSize = 10,
            params Expression<Func<TModel, object>>[] includeProperties)
        {
            IQueryable<TModel> query = dbSet;

            if (filter != null) query = query.Where(filter);

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return PaginatedList<TModel>.Create(orderBy(query), pageIndex, pageSize);
            }

            return PaginatedList<TModel>.Create(query, pageIndex, pageSize);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}