using Infrastructure.MediatorService;
using Infrastructure.MediatorService.Pipelines;
using Infrastructure.MediatorService.PublishStrategies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.MediatorService;

public class PreHandlerPipelineBehaviorTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public PreHandlerPipelineBehaviorTests()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<Log>();

        // Register mediator
        services.AddTransient<IMediator, Mediator>();
        services.AddTransient<INotificationPublisher, SyncStopOnExceptionPublisher>();

        // Register handler
        services.AddTransient<IRequestHandler<TestRequest, Unit>, TestRequestHandler>();

        // Register pipeline behavior
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PreHandlerPipelineBehavior<,>));

        // Register pre-handlers with different orders
        services.AddTransient<IPreHandlerPipelineBehavior<TestRequest>, FirstPreHandler>();
        services.AddTransient<IPreHandlerPipelineBehavior<TestRequest>, SecondPreHandler>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }

    private class Log : List<string> { }

    private class TestRequest
    {
        public string Message { get; set; } = string.Empty;
    }

    private class FirstPreHandler(Log log) : IPreHandlerPipelineBehavior<TestRequest>
    {
        public int Order => 1;

        public Task Handle(TestRequest request, CancellationToken cancellationToken)
        {
            log.Add($"FirstPreHandler:{request.Message}");
            return Task.CompletedTask;
        }
    }

    private class SecondPreHandler(Log log) : IPreHandlerPipelineBehavior<TestRequest>
    {
        public int Order => 2;

        public Task Handle(TestRequest request, CancellationToken cancellationToken)
        {
            log.Add($"SecondPreHandler:{request.Message}");
            return Task.CompletedTask;
        }
    }

    private class TestRequestHandler(Log log) : IRequestHandler<TestRequest, Unit>
    {
        public Task<Unit> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            log.Add($"Handler:{request.Message}");
            return Task.FromResult(Unit.Value);
        }
    }

    [Fact]
    public async Task PreHandlerPipelineBehavior_ExecutesAllPreHandlers_InOrder_BeforeHandler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var request = new TestRequest() { Message = "Hello" };
        var log = _serviceProvider.GetRequiredService<Log>();

        // Act
        await mediator.Send<TestRequest, Unit>(request);

        // Assert
        var expectedLog = new[]
        {
            "FirstPreHandler:Hello",
            "SecondPreHandler:Hello",
            "Handler:Hello"
        };

        Assert.Equal(expectedLog, log);
    }
}
