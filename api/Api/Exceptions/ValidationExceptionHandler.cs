using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Exceptions;

public class ValidationExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService problemDetailsService;

    public ValidationExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        this.problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not FluentValidation.ValidationException validationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
                Title = "One or more validation errors occurred",
                Detail = "See the 'errors' property for a list of specific error codes",
                Extensions =
                {
                    { "errors", GetValidationErrors(validationException) }
                }
            },
            Exception = exception
        });
    }

    private static List<string> GetValidationErrors(FluentValidation.ValidationException validationException)
    {
        var result = new List<string>(validationException.Errors.Count());

        foreach (var error in validationException.Errors)
        {
            result.Add(error.ErrorCode);
        }

        return result;
    }
}
