using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.ValidatorService;

public interface IValidatorService
{
    Task ValidateAndThrowAsync<T>(T instance, CancellationToken cancellationToken = default);
}

public class ServiceLocatorValidatorService : IValidatorService
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceLocatorValidatorService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ValidateAndThrowAsync<T>(T instance, CancellationToken cancellationToken = default)
    {
        // Get registered validator.
        var validator = _serviceProvider.GetService<AbstractValidator<T>>();

        if (validator is null)
        {
            throw new ValidatorNotFoundException(typeof(T));
        }

        await validator.Validate(instance, cancellationToken);

        if (!validator.IsValid)
        {
            throw new ValidationException(validator.Errors);
        }
    }
}
