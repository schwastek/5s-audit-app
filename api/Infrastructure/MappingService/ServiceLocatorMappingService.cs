using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.MappingService;

public interface IMappingService
{
    TDestination Map<TSource, TDestination>(TSource entity);
}

public class ServiceLocatorMappingService : IMappingService
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceLocatorMappingService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public TDestination Map<TSource, TDestination>(TSource entity)
    {
        // Get registered mapper
        var mapper = _serviceProvider.GetService<IMapper<TSource, TDestination>>();

        if (mapper is null)
        {
            throw new MapperNotFoundException(typeof(TSource), typeof(TDestination));
        }

        return mapper.Map(entity);
    }
}
