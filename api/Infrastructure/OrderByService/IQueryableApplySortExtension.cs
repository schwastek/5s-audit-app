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


        // Build Expression Tree and execute it
        foreach (var sortable in sortables)
        {
            // Produces lambda: "x => x.PropertyName".
            var selector = KeySelectorBuilder.BuildKeySelector<TSource>(sortable.PropertyName);

            // Get method and make it generic like "OrderBy<TSource, PropertyType>"
            var method = (sortable.SortDescending, isFirst) switch
            {
                (true, true) => QueryableMethods.OrderByDescending,
                (true, false) => QueryableMethods.ThenByDescending,
                (false, true) => QueryableMethods.OrderBy,
                (false, false) => QueryableMethods.ThenBy
            };
            method = method.MakeGenericMethod(sourceType, selector.Body.Type);

            // Build Expression like "TSource.OrderBy(p => p.Property)"
            var orderExpression = Expression.Call(method, source.Expression, selector);

            // Run Expression against collection
            source = source.Provider.CreateQuery<TSource>(orderExpression);

            isFirst = false;
        }

        return source;
    }
}
