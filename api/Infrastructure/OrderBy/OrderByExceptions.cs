using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.OrderBy;

/// <summary>
/// Base exception for all order-by related errors.
/// </summary>
public abstract class OrderByException : Exceptions.ApplicationException
{
    protected OrderByException(string message) : base(message) { }
}

/// <summary>
/// Thrown when the client requests ordering by a property that is not supported.
/// </summary>
public sealed class UnsupportedOrderByException : OrderByException
{
    public IReadOnlyList<string> UnsupportedProperties { get; }

    public UnsupportedOrderByException(IEnumerable<string> properties)
        : base(CreateMessage(properties))
    {
        UnsupportedProperties = properties.ToList().AsReadOnly();
    }

    private static string CreateMessage(IEnumerable<string> properties)
    {
        var list = string.Join(", ", properties);
        return $"The following order-by properties are not supported: {list}.";
    }
}
