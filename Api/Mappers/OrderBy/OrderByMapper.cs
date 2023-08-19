using System;
using System.Collections.Generic;

namespace Api.Mappers.OrderBy;

public abstract class OrderByMapper<TSource, TDestination>
{
    // You can create one to many mapping, e.g. "Name" (request) -> "FirstName" & "LastName" (model)
    protected Dictionary<string, IReadOnlyList<MappedOrderByParameter>> Mappings { get; init; } = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyList<MappedOrderByParameter> GetMappingFor(string propertyName)
    {
        if (!Mappings.TryGetValue(propertyName, out IReadOnlyList<MappedOrderByParameter>? mapping))
        {
            // If mapping does not exist, return empty list
            return new List<MappedOrderByParameter>();
        }

        return mapping;
    }
}
