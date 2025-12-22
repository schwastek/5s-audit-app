using System;
using System.Collections.Generic;

namespace Infrastructure.OrderBy;

/// <summary>
/// Represents a fully resolved and executable ORDER BY instruction.
/// </summary>
public record OrderByInstruction
{
    public required string PropertyName { get; init; }
    public bool SortDescending { get; init; }
}

public static class OrderByParser
{
    /// <summary>
    /// Parses a raw ORDER BY query string into a list of sort instructions.
    /// </summary>
    /// <param name="orderByQuery">
    /// A comma-separated ORDER BY expression from the request, for example:
    /// <c>"author asc, area desc"</c>.
    /// </param>
    /// <returns>
    /// A list of <see cref="OrderByInstruction"/> representing the parsed sort instructions.
    /// If the input is null or empty, an empty list is returned.
    /// </returns>
    /// <remarks>
    /// This method is responsible only for syntax parsing.
    /// It does not validate whether the properties are allowed
    /// and does not perform any mapping to domain or database properties.
    /// </remarks>
    public static IReadOnlyList<OrderByInstruction> Parse(string? orderByQuery)
    {
        if (string.IsNullOrWhiteSpace(orderByQuery)) return [];

        var parameters = new List<OrderByInstruction>();

        // "author asc, created DESC" -> ["author asc", "created DESC"]
        var afterSplit = orderByQuery
            .Split(",", StringSplitOptions.RemoveEmptyEntries);

        foreach (var orderByClause in afterSplit)
        {
            var trimmed = orderByClause.Trim();
            if (trimmed.Length == 0) continue;

            // "created desc" -> ["created", "desc"]
            var parts = trimmed.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var propertyName = parts[0];

            var sortDescending =
                parts.Length > 1 &&
                parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            var orderByParameter = new OrderByInstruction()
            {
                PropertyName = propertyName,
                SortDescending = sortDescending
            };

            parameters.Add(orderByParameter);
        }

        return parameters.AsReadOnly();
    }
}
