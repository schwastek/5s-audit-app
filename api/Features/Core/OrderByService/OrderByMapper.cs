using System;
using System.Collections.Generic;

namespace Features.Core.OrderByService;

public abstract class OrderByMapper<TSource, TDestination>
{
    // You can create one to many mapping, e.g. "FullName" (request) -> "FirstName" & "LastName" (DB)
    protected Dictionary<string, IReadOnlyList<MappedOrderByParameter>> Mappings { get; init; } = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyList<MappedOrderByParameter> GetMappingFor(string propertyName)
    {
        if (!Mappings.TryGetValue(propertyName, out IReadOnlyList<MappedOrderByParameter>? mapping))
        {
            // If mapping does not exist, return empty list
            return [];
        }

        return mapping;
    }
}
