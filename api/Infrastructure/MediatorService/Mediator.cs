using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.MediatorService;

/// <summary>
/// Defines a mediator that sends a request to a single handler and publishes notification to multiple handlers.
/// </summary>
public interface IMediator
{
    Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default);
    Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default);
}

/// <summary>
/// A basic implementation of the Mediator pattern that supports request handling and pipeline behaviors.
/// </summary>
public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INotificationPublisher _publisher;

    public Mediator(IServiceProvider serviceProvider, INotificationPublisher publisher)
    {
        _serviceProvider = serviceProvider;
        _publisher = publisher;
    }

    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        // The `CancellationToken` parameter lets a behavior override the original token.
        // This enables scenarios like timeout behaviors where a behavior can cancel the handler early.
        RequestHandlerDelegate<TResponse> handlerDelegate = (CancellationToken ct) =>
        {
            var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
            return handler.Handle(request, ct);
        };

        // Behaviors run in order they're registered.
        //
        // Behaviors: [A, B, C].
        // Handler: RealHandler.
        //
        // A.Handle(request, () =>
        //   B.Handle(request, () =>
        //     C.Handle(request, () =>
        //       RealHandler.Handle(request)
        //     )
        //   )
        // )
        //
        // A → calls B → calls C → calls actual handler.
        var behaviors = _serviceProvider.GetServices<IPipelineBehavior<TRequest, TResponse>>();

        foreach (var behavior in behaviors.Reverse())
        {
            // Start with the actual handler.
            var next = handlerDelegate;

            // In each iteration, define a new function that calls `next()`. Each new function is wrapping the previous one.
            // Each behavior calls `next()`, which is the previous delegate. So it's a chain of nested calls.
            // So, the handler is always executed last, but each behavior has a chance to run logic before and after the handler is executed.
            handlerDelegate = (ct) => behavior.Handle(request, next, ct);
        }

        // The outermost behavior A gets called first.
        // Behavior A wraps around the entire chain (including B and C and the real handler).
        return await handlerDelegate(cancellationToken);
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(notification);

        var handlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>();

        await _publisher.Publish(handlers, notification, cancellationToken);
    }
}
