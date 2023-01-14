using Api.Services;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Api.Helpers;

public static class IQueryableExtensions
{
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
        Dictionary<string, PropertyMappingValue> mappingDictionary)
    {
        if (string.IsNullOrWhiteSpace(orderBy)) return source;

        string orderByQuery = OrderQueryBuilder.CreateOrderQuery<T>(orderBy, mappingDictionary);

        return source.OrderBy(orderByQuery);
    }
}
