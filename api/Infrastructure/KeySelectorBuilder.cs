using System;
using System.Linq.Expressions;

namespace Infrastructure;

public static class KeySelectorBuilder
{
    /// <summary>
    /// Builds a strongly typed lambda expression that selects a property by name.
    /// </summary>
    /// <typeparam name="T">The type of the source object.</typeparam>
    /// <param name="propertyName">The name of the property to select.</param>
    /// <returns>
    /// An expression equivalent to <c>x => x.PropertyName</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the specified property does not exist on <typeparamref name="T"/>.
    /// </exception>
    public static LambdaExpression BuildKeySelector<T>(string propertyName)
    {
        // Parameter expression: "x".
        var parameter = Expression.Parameter(typeof(T), "x");

        // Property access: "x.PropertyName".
        var propertyAccess = Expression.Property(parameter, propertyName);

        // Lambda: "x => x.PropertyName".
        var lambda = Expression.Lambda(
            typeof(Func<,>).MakeGenericType(typeof(T), propertyAccess.Type),
            propertyAccess,
            parameter
        );

        return lambda;
    }
}
