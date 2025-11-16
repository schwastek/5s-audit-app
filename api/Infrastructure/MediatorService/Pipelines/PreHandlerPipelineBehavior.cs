using Infrastructure.MediatorService;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.MediatorService.Pipelines;

/// <summary>
/// Interface for defining the execution order of pre- and post-handlers.
/// </summary>
/// <remarks>
/// The <see cref="Order"/> property determines the order in which pre- or post-handlers are executed.
/// Lower values are executed before higher values.
/// </remarks>
public interface IOrderedHandler
{
    int Order { get; }
}

/// <summary>
/// Defines a pre-processing action that runs before the request handler is invoked.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
public interface IPreHandlerPipelineBehavior<in TRequest> : IOrderedHandler
{
    Task Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Pipeline behavior that runs all registered <see cref="IPreHandlerPipelineBehavior{in TRequest}"/> 
/// in the specified order before the request handler is executed.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class PreHandlerPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IPreHandlerPipelineBehavior<TRequest>> _preProcessors;

    public PreHandlerPipelineBehavior(IEnumerable<IPreHandlerPipelineBehavior<TRequest>> preProcessors)
    {
        _preProcessors = preProcessors.OrderBy(p => p.Order).ToList();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        foreach (var processor in _preProcessors)
        {
            await processor.Handle(request, cancellationToken);
        }

        var response = await next(cancellationToken);

        return response;
    }
}
