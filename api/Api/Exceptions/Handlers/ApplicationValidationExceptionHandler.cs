using Features.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Exceptions.Handlers;

public class ApplicationValidationExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService problemDetailsService;

    public ApplicationValidationExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        this.problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is not ApplicationValidationException applicationValidationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var problemDetails = new CustomValidationProblemDetails()
        {
            Detail = applicationValidationException.Message,
            Errors = GetValidationErrors(applicationValidationException.Errors)
        };

        var result = await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });

        return result;
    }

    private static ICollection<string> GetValidationErrors(IEnumerable<string> errors)
    {
        var result = new SortedSet<string>(errors, StringComparer.Ordinal);

        return result;
    }
}
