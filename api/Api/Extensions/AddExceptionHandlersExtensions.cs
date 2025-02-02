using Api.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions;

public static class AddExceptionHandlersExtensions
{
    public static void AddExceptionHandlers(this IServiceCollection services)
    {
        AddProblemDetails(services);
        AddHandlers(services);
    }

    public static void AddProblemDetails(this IServiceCollection services)
    {
        services.AddSingleton<IProblemDetailsWriter, CustomProblemDetailsWriter>();

        // Return the Problem Details format for non-successful responses.
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = ctx =>
            {
                // Add requested path to every Problem Details response.
                ctx.ProblemDetails.Instance = ctx.HttpContext.Request.Path;
            };
        });
    }

    public static void AddHandlers(this IServiceCollection services)
    {
        // Exception handlers are processed in the order they're registered.
        // Each handler attempts to process the error, and if not handled, the next in the chain is executed.
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<DomainValidationExceptionHandler>();
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<DefaultExceptionHandler>();
    }
}
