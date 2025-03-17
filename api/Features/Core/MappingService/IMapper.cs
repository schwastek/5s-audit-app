namespace Features.Core.MappingService;

public interface IMapper<TSource, TDestination>
{
    TDestination Map(TSource src);
}
