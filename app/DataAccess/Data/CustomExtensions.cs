using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DataAccess.Data
{
    public static class CustomExtensions
    {
        public static IQueryable Query(this ApplicationDbContext context, string entityName)
        {
            return context.Query(context.Model.FindEntityType(entityName).ClrType);
        }

        public static IQueryable Query(this ApplicationDbContext context, Type entityType)
        {
            return (IQueryable)((IDbSetCache)context).GetOrAddSet(context.GetDependencies().SetSource, entityType);
        }

        public static IQueryable<T> Include<T>(this IQueryable<T> source, IEnumerable<string> navigationPropertyPaths)
            where T : class
        {
            return navigationPropertyPaths.Aggregate(source, (query, path) => query.Include(path));
        }
    }
}