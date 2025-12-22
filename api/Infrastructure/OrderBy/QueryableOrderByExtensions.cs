using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.OrderBy;

public static class QueryableOrderByExtensions
{
    public static IQueryable<TSource> ApplyOrderBy<TSource>(this IQueryable<TSource> source, IEnumerable<OrderByInstruction>? instructions)
    {
        return OrderByExecutor.ApplyOrderBy(source, instructions);
    }
}
