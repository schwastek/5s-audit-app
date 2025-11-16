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

public class PostHandlerPipelineBehaviorTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public PostHandlerPipelineBehaviorTests()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<Log>();

        // Register mediator
        services.AddTransient<IMediator, Mediator>();
        services.AddTransient<INotificationPublisher, SyncStopOnExceptionPublisher>();

        // Register handler
        services.AddTransient<IRequestHandler<TestRequest, string>, TestRequestHandler>();

        // Register pipeline behavior
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PostHandlerPipelineBehavior<,>));

        // Register post-handlers with different orders
        services.AddTransient<IPostHandlerPipelineBehavior<TestRequest, string>, FirstPostHandler>();
        services.AddTransient<IPostHandlerPipelineBehavior<TestRequest, string>, SecondPostHandler>();

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

    private class TestRequestHandler(Log log) : IRequestHandler<TestRequest, string>
    {
        public Task<string> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            log.Add($"Handler:{request.Message}");
            return Task.FromResult(request.Message);
        }
    }

    private class FirstPostHandler(Log log) : IPostHandlerPipelineBehavior<TestRequest, string>
    {
        public int Order => 1;

        public Task Handle(TestRequest request, string response, CancellationToken cancellationToken)
        {
            log.Add($"FirstPostHandler:{request.Message}:{response}");
            return Task.CompletedTask;
        }
    }

    private class SecondPostHandler(Log log) : IPostHandlerPipelineBehavior<TestRequest, string>
    {
        public int Order => 2;

        public Task Handle(TestRequest request, string response, CancellationToken cancellationToken)
        {
            log.Add($"SecondPostHandler:{request.Message}:{response}");
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task PostHandlerPipelineBehavior_ExecutesAllPostHandlers_InOrder_AfterHandler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var request = new TestRequest() { Message = "Hello" };
        var log = _serviceProvider.GetRequiredService<Log>();

        // Act
        var result = await mediator.Send<TestRequest, string>(request);

        // Assert
        Assert.Equal("Hello", result);

        var expectedLog = new[]
        {
            "Handler:Hello",
            "FirstPostHandler:Hello:Hello",
            "SecondPostHandler:Hello:Hello"
        };

        Assert.Equal(expectedLog, log);
    }
}
