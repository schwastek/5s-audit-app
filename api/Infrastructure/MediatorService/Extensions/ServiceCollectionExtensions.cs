using Infrastructure.MediatorService.PublishStrategies;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MediatorService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatorRequiredServices(this IServiceCollection services)
    {
        services.AddTransient<IMediator, Mediator>();
        services.AddTransient<INotificationPublisher, SyncStopOnExceptionPublisher>();

        return services;
    }
}
