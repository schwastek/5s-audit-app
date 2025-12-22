using System.Collections.Generic;

namespace Infrastructure.OrderBy;

/// <summary>
/// Translates request-facing ORDER BY instructions into executable domain sort instructions
/// using a provided order-by mapping definition.
/// </summary>
/// <remarks>
/// <para>
/// If one or more requested ORDER BY properties are not supported by the provided mapping,
/// exception <see cref="UnsupportedOrderByException"/> is thrown containing all unsupported
/// property names.
/// </para>
/// </remarks>
public static class OrderByMapper
{
    public static IReadOnlyList<OrderByInstruction> Map(
        IReadOnlyList<OrderByInstruction> orderByParameters,
        IOrderByMap map)
    {
        var instructions = new List<OrderByInstruction>();
        var unsupported = new List<string>();

        foreach (var orderByParameter in orderByParameters)
        {
            var mapping = map.GetMappingFor(orderByParameter.PropertyName);

            if (mapping is null)
            {
                unsupported.Add(orderByParameter.PropertyName);
                continue;
            }

            foreach (var mappedProperty in mapping)
            {
                var isDescending = mappedProperty.ReverseDirection
                    ? !orderByParameter.SortDescending
                    : orderByParameter.SortDescending;

                var orderByInstruction = new OrderByInstruction
                {
                    PropertyName = mappedProperty.PropertyName,
                    SortDescending = isDescending
                };

                instructions.Add(orderByInstruction);
            }
        }

        if (unsupported.Count > 0)
        {
            throw new UnsupportedOrderByException(unsupported);
        }

        return instructions.AsReadOnly();
    }
}

/// <summary>
/// Provides mapping from request-facing ORDER BY property names 
/// to domain or database sort targets.
/// </summary>
/// <remarks>
/// This interface represents a read-only whitelist of allowed ORDER BY fields.
/// It contains no parsing, validation, or sorting logic - only mapping data.
/// </remarks>
public interface IOrderByMap
{
    IReadOnlyList<OrderByTarget>? GetMappingFor(string propertyName);
}

/// <summary>
/// Marker interface for an ORDER BY map scoped to a specific context or entity.
/// </summary>
/// <typeparam name="TContext">
/// The domain or entity type this map applies to.
/// </typeparam>
/// <remarks>
/// This interface does not add behavior.
/// The generic type parameter exists solely to make dependency injection straightforward.
/// <para>
/// It allows multiple ORDER BY maps to be registered and resolved by context
/// (for example: User entity) without using keyed services, string-based lookups or factories.
/// </para>
/// </remarks>
public interface IOrderByMap<TContext> : IOrderByMap { }

/// <summary>
/// Defines how a request-facing ORDER BY field maps to a domain or database property.
/// </summary>
/// <remarks>
/// A single request property can map to one or more domain properties.
/// Sort direction might be reversed to preserve the meaning of the request-facing field.
/// <para>
/// Examples:
/// <br/>
/// <c>fullName</c> (request) → <c>firstName</c> and <c>lastName</c> (database)
/// <br/>
/// <c>age asc</c> (request) → <c>dateOfBirth desc</c> (database),
/// because younger age (20, 21...) means later birth date (2001, 2000...).
/// </para>
/// </remarks>
public sealed record OrderByTarget
{
    public required string PropertyName { get; init; }
    public bool ReverseDirection { get; init; }
}
