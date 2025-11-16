using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.OrderByService;

public static class IQueryableApplySortExtension
{
    public static IQueryable<TSource> ApplySort<TSource>(this IQueryable<TSource> source, IEnumerable<Sortable> sortables)
    {
        if (!sortables.Any()) return source;

        var isFirst = true;
        var sourceType = typeof(TSource);

        // Define parameter passed to lambda expression "p =>"
        var parameter = Expression.Parameter(sourceType, "p");

        // Build Expression Tree and execute it
        foreach (var sortable in sortables)
        {
            // Retrieve the value of property
            var propertyAccess = Expression.Property(parameter, sortable.PropertyName);

            // "p => p.Property"
            var selector = Expression.Lambda(propertyAccess, parameter);

            // Get method and make it generic like "OrderBy<TSource, PropertyType>"
            var method = (sortable.SortDescending, isFirst) switch
            {
                (true, true) => QueryableMethods.OrderByDescending,
                (true, false) => QueryableMethods.ThenByDescending,
                (false, true) => QueryableMethods.OrderBy,
                (false, false) => QueryableMethods.ThenBy
            };
            method = method.MakeGenericMethod(sourceType, propertyAccess.Type);

            // Build Expression like "TSource.OrderBy(p => p.Property)"
            var orderExpression = Expression.Call(method, source.Expression, selector);

            // Run Expression against collection
            source = source.Provider.CreateQuery<TSource>(orderExpression);

            isFirst = false;
        }

        return source;
    }
}
