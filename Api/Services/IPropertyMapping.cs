namespace Api.Services
{
    public interface IPropertyMapping
    {
        // You can't declare `IList<PropertyMapping<TSource, TDestination>>` (a list with multiple generic types).
        // `PropertyMapping<TSource, TDestination>` doesn't have parent class that you could cast to.
        // As a solution, define a common interface that it inherits from. Then you can do:
        // `IList<IPropertyMapping>`
    }
}
