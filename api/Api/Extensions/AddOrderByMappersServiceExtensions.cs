using Domain;
using Features.Audits.List;
using Features.Audits.Mappers;
using Infrastructure.OrderByService;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions;

public static class AddOrderByMappersServiceExtensions
{
    public static void AddOrderByMappers(this IServiceCollection services)
    {
        // Universal service
        services.AddSingleton(typeof(OrderByMappingService<,>));

        // For Audits
        services.AddSingleton<OrderByMapper<ListAuditsQuery, Audit>, AuditOrderByMapper>();
    }
}
