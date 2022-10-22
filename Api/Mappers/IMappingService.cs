namespace Api.Mappers
{
    public interface IMappingService
    {
        TDestination Map<TSource, TDestination>(TSource entity);
    }
}
