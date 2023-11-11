using Api.Common;
using System.Linq;

namespace Api.Extensions;

public static class ApplyPagingIQueryableExtension
{
    // Note: `Skip`/`Take` should be invoked after `OrderBy`
    public static IQueryable<TSource> ApplyPaging<TSource>(this IQueryable<TSource> source, IPageableQuery query)
    {
        var items = source
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize);

        return items;
    }
}
