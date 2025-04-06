using Features.Core.MediatorService;
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
        services.AddSingleton(sp => new Log());
        services.AddSingleton<IRequestHandler<RequestWithResponse, string>, HandlerWithResponse>();
        services.AddTransient<IRequestHandler<RequestWithNoResponse, Unit>, HandlerWithNoResponse>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behavior1<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behavior2<,>));
        services.AddSingleton<IMediator, Mediator>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }

    public class Log : List<string> { }

    public class RequestWithResponse : IRequest<string>
    {
        public string Message { get; set; } = string.Empty;
    }

    public class HandlerWithResponse : IRequestHandler<RequestWithResponse, string>
    {
        private readonly Log _log;

        public HandlerWithResponse(Log log) => _log = log;

        public Task<string> Handle(RequestWithResponse request, CancellationToken cancellationToken)
        {
            _log.Add("Handler");
            _log.Add($"Request message: {request.Message}");

            return Task.FromResult(request.Message);
        }
    }

    public class RequestWithNoResponse : IRequest<Unit>
    {
        public string Message { get; set; } = string.Empty;
    }

    public class HandlerWithNoResponse : IRequestHandler<RequestWithNoResponse, Unit>
    {
        private readonly Log _log;

        public HandlerWithNoResponse(Log log) => _log = log;

        public Task<Unit> Handle(RequestWithNoResponse request, CancellationToken cancellationToken)
        {
            _log.Add("Handler");
            _log.Add($"Request message: {request.Message}");

            return Task.FromResult(Unit.Value);
        }
    }

    public class Behavior1<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Log _log;

        public Behavior1(Log log) => _log = log;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _log.Add("Behavior1 - Before");
            var result = await next(cancellationToken);
            _log.Add("Behavior1 - After");
            return result;
        }
    }

    public class Behavior2<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Log _log;

        public Behavior2(Log log) => _log = log;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _log.Add("Behavior2 - Before");
            var result = await next(cancellationToken);
            _log.Add("Behavior2 - After");
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
