using Features.Core.MediatorService;
using Features.Core.MediatorService.PublishStrategies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.MediatorService;

public class SendTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public SendTests()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<Log>();
        services.AddSingleton<INotificationPublisher, SyncStopOnExceptionPublisher>();
        services.AddTransient<IRequestHandler<RequestWithResponse, string>, HandlerWithResponse>();
        services.AddTransient<IRequestHandler<RequestWithNoResponse, Unit>, HandlerWithNoResponse>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behavior1<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behavior2<,>));
        services.AddTransient<IMediator, Mediator>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }

    private class Log : List<string> { }

    private class RequestWithResponse
    {
        public string Message { get; set; } = string.Empty;
    }

    private class HandlerWithResponse(Log log) : IRequestHandler<RequestWithResponse, string>
    {
        public Task<string> Handle(RequestWithResponse request, CancellationToken cancellationToken)
        {
            log.Add("Handler");
            log.Add($"Request message: {request.Message}");

            return Task.FromResult(request.Message);
        }
    }

    private class RequestWithNoResponse
    {
        public string Message { get; set; } = string.Empty;
    }

    private class HandlerWithNoResponse(Log log) : IRequestHandler<RequestWithNoResponse, Unit>
    {
        public Task<Unit> Handle(RequestWithNoResponse request, CancellationToken cancellationToken)
        {
            log.Add("Handler");
            log.Add($"Request message: {request.Message}");

            return Task.FromResult(Unit.Value);
        }
    }

    private class Behavior1<TRequest, TResponse>(Log log) : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            log.Add("Behavior1 - Before");
            var result = await next(cancellationToken);
            log.Add("Behavior1 - After");
            return result;
        }
    }

    private class Behavior2<TRequest, TResponse>(Log log) : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            log.Add("Behavior2 - Before");
            var result = await next(cancellationToken);
            log.Add("Behavior2 - After");
            return result;
        }
    }

    [Fact]
    public async Task Handles_RequestWithResponse_Successfully()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var request = new RequestWithResponse() { Message = "Hello" };
        var log = _serviceProvider.GetRequiredService<Log>();

        // Act
        var result = await mediator.Send<RequestWithResponse, string>(request);

        // Assert
        Assert.Equal("Hello", result);

        var expectedLog = new[]
        {
            "Behavior1 - Before",
            "Behavior2 - Before",
            "Handler",
            "Request message: Hello",
            "Behavior2 - After",
            "Behavior1 - After"
        };

        Assert.Equal(expectedLog, log);
    }

    [Fact]
    public async Task Handles_RequestWithNoResponse_Successfully()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var request = new RequestWithNoResponse() { Message = "Hello" };
        var log = _serviceProvider.GetRequiredService<Log>();

        // Act
        await mediator.Send<RequestWithNoResponse, Unit>(request);

        // Assert
        var expectedLog = new[]
        {
            "Behavior1 - Before",
            "Behavior2 - Before",
            "Handler",
            "Request message: Hello",
            "Behavior2 - After",
            "Behavior1 - After"
        };

        Assert.Equal(expectedLog, log);
    }
}
