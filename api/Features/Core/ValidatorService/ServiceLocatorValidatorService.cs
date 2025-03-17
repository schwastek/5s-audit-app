using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Core.ValidatorService;

public interface IValidatorService
{
    Task ValidateAndThrowAsync<T>(T instance, CancellationToken cancellationToken = default);
}

public class ServiceLocatorValidatorService : IValidatorService
{
    private readonly IServiceProvider serviceProvider;

    public ServiceLocatorValidatorService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task ValidateAndThrowAsync<T>(T instance, CancellationToken cancellationToken = default)
    {
        // Get registered validator.
        var validator = serviceProvider.GetService<IValidator<T>>();

        if (validator is null)
        {
            throw new ValidatorNotFoundException(typeof(T));
        }

        await validator.ValidateAndThrowAsync(instance, cancellationToken);
    }
}
