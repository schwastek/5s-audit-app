using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.OrderByService;

public class OrderByMappingService<TSource, TDestination>
{
    private readonly OrderByMapper<TSource, TDestination> _mapper;

    public OrderByMappingService(OrderByMapper<TSource, TDestination> mapper)
    {
        _mapper = mapper;
    }

    public IReadOnlyList<Sortable> Map(string? orderByQuery)
    {
        if (string.IsNullOrWhiteSpace(orderByQuery))
        {
            return [];
        }

        var orderByParameters = CreateOrderByParameters(orderByQuery);
        var sortables = CreateSortables(orderByParameters);

        return sortables;
    }

    public bool ValidMappingExists(string? orderByQuery)
    {
        if (string.IsNullOrWhiteSpace(orderByQuery))
        {
            return true;
        }

        var orderByParameters = CreateOrderByParameters(orderByQuery);

        foreach (var orderByParameter in orderByParameters)
        {
            var mapping = _mapper.GetMappingFor(orderByParameter.PropertyName);

            if (!mapping.Any())
            {
                return false;
            }
        }

        return true;
    }

    private static List<OrderByParameter> CreateOrderByParameters(string orderByQuery)
    {
        var parameters = new List<OrderByParameter>();

        // "author asc, created DESC" -> ["author asc", "created desc"]
        var afterSplit = orderByQuery
            .ToLower()
            .Split(",");

        foreach (var orderByClause in afterSplit)
        {
            var trimmed = orderByClause.Trim();

            // "created desc" -> "created" or "created" -> "created"
            var space = trimmed.IndexOf(' ');
            var propertyName = space == -1
                ? trimmed
                : trimmed.Remove(space);

            var sortDescending = trimmed.EndsWith(" desc");

            var orderByParamer = new OrderByParameter()
            {
                PropertyName = propertyName,
                SortDescending = sortDescending
            };

            parameters.Add(orderByParamer);
        }

        // It's easier with Records
        var unique = parameters
            .Distinct()
            .ToList();

        return unique;
    }

    private List<Sortable> CreateSortables(IEnumerable<OrderByParameter> orderByParameters)
    {
        var sortables = new List<Sortable>();

        foreach (var orderByParameter in orderByParameters)
        {
            var mapping = _mapper.GetMappingFor(orderByParameter.PropertyName);
            var isDescending = orderByParameter.SortDescending;

            foreach (var mappedParameter in mapping)
            {
                // Reverse sort order if necessary
                if (mappedParameter.Reverse)
                {
                    isDescending = !isDescending;
                }

                var sortable = new Sortable()
                {
                    PropertyName = mappedParameter.PropertyName,
                    SortDescending = isDescending
                };

                sortables.Add(sortable);
            }
        }

        return sortables;
    }
}
