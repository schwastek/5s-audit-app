using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Core.MediatorService;

/// <summary>
/// Marker interface for a request that expects a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <remarks>
/// Technically you don't need `IRequest<TResponse>` and `where TRequest : IRequest<TResponse>` for your mediator to work.
/// You can just resolve the handler from DI using both `TRequest` and `TResponse`. As long as there's a handler registered, it works.
/// So what does `IRequest<TResponse>` + constraint really add?
/// Prevents mismatches between `TRequest` and `TResponse`.
/// Tells the developer and compiler: "This is a query that returns that DTO."
/// It's mostly developer safety and DX (developer experience).
/// </remarks>
/// <typeparam name="TResponse">Response type.</typeparam>
public interface IRequest<out TResponse> { }

/// <summary>
/// Handles a request of type <typeparamref name="TRequest"/> and returns a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request.</typeparam>
/// <typeparam name="TResponse">The type of response.</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Delegate representing the next step in the request handler pipeline.
/// </summary>
/// <remarks>
/// It accepts a `CancellationToken` so that behaviors can inject their own token,
/// for example to enforce a timeout or cancel the operation early.
/// </remarks>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <returns>A task containing the response.</returns>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>(CancellationToken cancellationToken);

/// <summary>
/// Defines a pipeline behavior that wraps the execution of a request handler.
/// </summary>
/// <remarks>
/// Executes logic before and/or after the next delegate in the pipeline.
/// </remarks>
/// <typeparam name="TRequest">The type of request.</typeparam>
/// <typeparam name="TResponse">The type of response.</typeparam>
public interface IPipelineBehavior<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

/// <summary>
/// Represents a void-like return type for requests that do not return a value.
/// </summary>
public readonly struct Unit : IEquatable<Unit>
{
    public static readonly Unit Value = new();

    public override bool Equals(object? obj) => obj is Unit;
    public bool Equals(Unit other) => true;
    public override int GetHashCode() => 0;
    public override string ToString() => "()";

    public static bool operator ==(Unit left, Unit right) => true;
    public static bool operator !=(Unit left, Unit right) => false;
}

public interface IMediator
{
    Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest<TResponse>;
}

/// <summary>
/// A basic implementation of the Mediator pattern that supports request handling and pipeline behaviors.
/// </summary>
public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
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
}
