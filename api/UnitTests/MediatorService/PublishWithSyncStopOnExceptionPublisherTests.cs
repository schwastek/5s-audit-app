using Features.Core.MediatorService;
using Features.Core.MediatorService.PublishStrategies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.MediatorService;

public class PublishWithSyncStopOnExceptionPublisherTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public PublishWithSyncStopOnExceptionPublisherTests()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<CallOrder>();
        services.AddTransient<INotificationPublisher, SyncStopOnExceptionPublisher>();
        services.AddTransient<INotificationHandler<Notification>, FirstHandler>();
        services.AddTransient<INotificationHandler<Notification>, SecondHandler>();
        services.AddTransient<INotificationHandler<Notification>, ThirdHandler>();
        services.AddTransient<IMediator, Mediator>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }

    private class CallOrder : List<string> { }

    private class Notification { }

    private class FirstHandler(CallOrder callOrder) : INotificationHandler<Notification>
    {
        public Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            callOrder.Add(nameof(FirstHandler));
            return Task.CompletedTask;
        }
    }

    private class SecondHandler(CallOrder callOrder) : INotificationHandler<Notification>
    {
        public Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            callOrder.Add(nameof(SecondHandler));
            return Task.CompletedTask;
        }
    }

    private class ThirdHandler(CallOrder callOrder) : INotificationHandler<Notification>
    {
        public Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            callOrder.Add(nameof(ThirdHandler));
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Publish_ShouldExecuteHandlersSequentially()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var notification = new Notification();
        var callOrder = _serviceProvider.GetRequiredService<CallOrder>();

        // Act
        await mediator.Publish(notification);

        // Assert
        var expectedCallOrder = new[]
        {
            nameof(FirstHandler),
            nameof(SecondHandler),
            nameof(ThirdHandler)
        };

        Assert.Equal(expectedCallOrder, callOrder);
    }
}
