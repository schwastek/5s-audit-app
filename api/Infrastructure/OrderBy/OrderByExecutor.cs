using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.OrderBy;

public static class OrderByExecutor
{
    /// <summary>
    /// Applies dynamic ordering to an <see cref="IQueryable{T}"/> based on the provided sort instructions.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the query.</typeparam>
    /// <param name="source">The source query to apply ordering to.</param>
    /// <param name="instructions">
    /// A sequence of sort instructions that specify property names and sort directions.
    /// The order of items determines the order in which sorting is applied.
    /// </param>
    /// <returns>
    /// A new <see cref="IQueryable{T}"/> with the specified ordering applied.
    /// If <paramref name="instructions"/> is null or empty, the original query is returned.
    /// </returns>
    /// <remarks>
    /// This method builds a composed LINQ expression and relies on deferred execution.
    /// The query is not executed until the returned <see cref="IQueryable{T}"/> is enumerated.
    /// </remarks>
    public static IQueryable<TSource> ApplyOrderBy<TSource>(IQueryable<TSource> source, IEnumerable<OrderByInstruction>? instructions)
    {
        if (instructions is null) return source;

        // Materialize once to preserve order and avoid re-enumeration.
        var list = instructions as IReadOnlyList<OrderByInstruction> ?? instructions.ToList();

        if (list.Count == 0) return source;

        // Build a chained LINQ expression tree.
        var queryExpression = source.Expression;

        for (int i = 0; i < list.Count; i++)
        {
            var sortable = list[i];

            // First ordering uses OrderBy; subsequent ones use ThenBy.
            var isFirst = i == 0;

            // Builds lambda: x => x.PropertyName.
            var keySelector = KeySelectorBuilder.BuildKeySelector<TSource>(sortable.PropertyName);

            // Get ordering method name.
            var methodName = (isFirst, sortable.SortDescending) switch
            {
                (true, false) => nameof(Queryable.OrderBy),
                (true, true) => nameof(Queryable.OrderByDescending),
                (false, false) => nameof(Queryable.ThenBy),
                (false, true) => nameof(Queryable.ThenByDescending)
            };

            // Chain ordering onto the existing query expression.
            // Build an expression-tree call equivalent to: Queryable.OrderBy<TSource, TKey>(queryExpression, keySelector).
            // This becomes the next link in the chain, just like writing: queryExpression.OrderBy(x => x.PropertyName).ThenBy(...).
            queryExpression = Expression.Call(
                type: typeof(Queryable),
                methodName: methodName,
                typeArguments: [typeof(TSource), keySelector.Body.Type],
                arguments: [queryExpression, keySelector]
            );
        }

        // Create the final query and pass it to the underlying provider (EF, LINQ-to-Objects...).
        // The provider will later translate and execute this expression when the query is enumerated.
        var result = source.Provider.CreateQuery<TSource>(queryExpression);

        return result;
    }
}
