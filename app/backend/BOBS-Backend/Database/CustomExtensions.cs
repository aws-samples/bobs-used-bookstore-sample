using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BOBS_Backend.Database
{
    public static partial class CustomExtensions
    {
        public static IQueryable Query(this DatabaseContext context, string entityName) =>
            context.Query(context.Model.FindEntityType(entityName).ClrType);

        public static IQueryable Query(this DatabaseContext context, Type entityType) =>
            (IQueryable)((IDbSetCache)context).GetOrAddSet(context.GetDependencies().SetSource, entityType);

        public static IQueryable<T> Include<T>(this IQueryable<T> source, IEnumerable<string> navigationPropertyPaths)
            where T : class
        {
            return navigationPropertyPaths.Aggregate(source, (query, path) => query.Include(path));
        }
    }
}
