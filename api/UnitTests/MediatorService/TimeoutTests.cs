using Features.Core.MediatorService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.MediatorService;

public class TimeoutTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public TimeoutTests()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<Dependency>(sp => new Dependency());
        services.AddTransient<IRequestHandler<TimeoutRequest, Unit>, TimeoutRequestHandler>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TimeoutBehavior<,>));
        services.AddSingleton<IMediator, Mediator>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }

    public class Dependency
    {
        public bool Called { get; set; }
    }

    public class TimeoutRequest : IRequest<Unit> { }

    public class TimeoutRequestHandler : IRequestHandler<TimeoutRequest, Unit>
    {
        private readonly Dependency _dependency;

        public TimeoutRequestHandler(Dependency dependency) => _dependency = dependency;

        public async Task<Unit> Handle(TimeoutRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(2000, cancellationToken);

            _dependency.Called = true;

            return Unit.Value;
        }
    }

    public class TimeoutBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            using var cts = new CancellationTokenSource(500);
            return await next(cts.Token);
        }
    }

    [Fact]
    public async Task TimeoutBehavior_Cancels_Handler()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var request = new TimeoutRequest();
        var dependency = _serviceProvider.GetRequiredService<Dependency>();

        // Act
        var exception = await Assert.ThrowsAsync<TaskCanceledException>(() => mediator.Send<TimeoutRequest, Unit>(request));

        // Assert
        Assert.NotNull(exception);
        Assert.IsAssignableFrom<TaskCanceledException>(exception);
        Assert.False(dependency.Called);
    }
}
