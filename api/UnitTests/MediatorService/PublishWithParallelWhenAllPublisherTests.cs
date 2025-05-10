using Features.Core.MediatorService;
using Features.Core.MediatorService.PublishStrategies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.MediatorService;

public class PublishWithParallelWhenAllPublisherTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public PublishWithParallelWhenAllPublisherTests()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<INotificationPublisher, ParallelWhenAllPublisher>();
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

    private class Notification { }

    private class FirstHandler : INotificationHandler<Notification>
    {
        public async Task Handle(Notification notification, CancellationToken cancellationToken)
            => await Task.Delay(200, cancellationToken);
    }

    private class SecondHandler : INotificationHandler<Notification>
    {
        public async Task Handle(Notification notification, CancellationToken cancellationToken)
            => await Task.Delay(200, cancellationToken);
    }

    private class ThirdHandler : INotificationHandler<Notification>
    {
        public async Task Handle(Notification notification, CancellationToken cancellationToken)
            => await Task.Delay(200, cancellationToken);
    }

    [Fact]
    public async Task Publish_ExecuteHandlersConcurrently()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var notification = new Notification();
        var timer = new Stopwatch();

        // Act
        timer.Start();
        await mediator.Publish(notification);
        timer.Stop();

        // Assert
        var elapsed = timer.ElapsedMilliseconds;
        var expected = 550;

        Assert.True(elapsed < expected, $"Expected: {expected}. Elapsed: {elapsed}.");
    }
}
