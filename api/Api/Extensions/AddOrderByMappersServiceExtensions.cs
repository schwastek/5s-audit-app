using Domain;
using Features.Audits.Mappers;
using Infrastructure.OrderBy;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions;

public static class AddOrderByMappersServiceExtensions
{
    public static void AddOrderByMappers(this IServiceCollection services)
    {
        // Universal service
        services.AddSingleton(typeof(IOrderByService<>), typeof(OrderByService<>));

        // For Audits
        services.AddSingleton<IOrderByMap<Audit>, AuditOrderByMapConfiguration>();
    }
}
