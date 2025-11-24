using Infrastructure.ValidatorService;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.MediatorService.Pipelines;

public class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<AbstractValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<AbstractValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(cancellationToken);
        }

        await Task.WhenAll(_validators.Select(x => x.Validate(request, cancellationToken)));
        var errors = _validators.Where(x => !x.IsValid).SelectMany(x => x.Errors).ToList();

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }

        return await next(cancellationToken);
    }
}
