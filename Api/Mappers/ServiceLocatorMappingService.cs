using Microsoft.Extensions.DependencyInjection;
using System;

namespace Api.Mappers
{
    public class ServiceLocatorMappingService : IMappingService
    {
        private readonly IServiceProvider serviceProvider;

        public ServiceLocatorMappingService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public TDestination Map<TSource, TDestination>(TSource entity)
        {
            // Get registered mapper
            var mapper = serviceProvider.GetService<IMapper<TSource, TDestination>>();

            if (mapper == null)
            {
                throw new MapperNotFoundException(typeof(TSource), typeof(TDestination));
            }

            return mapper.Map(entity);
        }
    }
}
