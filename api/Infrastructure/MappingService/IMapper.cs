namespace Infrastructure.MappingService;

public interface IMapper<TSource, TDestination>
{
    TDestination Map(TSource src);
}
