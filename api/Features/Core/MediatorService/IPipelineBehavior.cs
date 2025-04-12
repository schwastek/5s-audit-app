using System.Threading;
using System.Threading.Tasks;

namespace Features.Core.MediatorService;

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
