using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Core.MediatorService.Pipelines;

/// <summary>
/// Defines a post-processing action that runs after the request handler has returned a response.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IPostHandlerPipelineBehavior<in TRequest, in TResponse> : IOrderedHandler
{
    Task Handle(TRequest request, TResponse response, CancellationToken cancellationToken);
}

/// <summary>
/// Pipeline behavior that runs all registered <see cref="IPostHandlerPipelineBehavior{TRequest, TResponse}"/> 
/// in the specified order after the request handler has executed.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class PostHandlerPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IPostHandlerPipelineBehavior<TRequest, TResponse>> _postProcessors;

    public PostHandlerPipelineBehavior(IEnumerable<IPostHandlerPipelineBehavior<TRequest, TResponse>> postProcessors)
    {
        _postProcessors = postProcessors.OrderBy(p => p.Order).ToList();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next(cancellationToken);

        foreach (var processor in _postProcessors)
        {
            await processor.Handle(request, response, cancellationToken);
        }

        return response;
    }
}
