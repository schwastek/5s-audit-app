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

        var problemDetails = new CustomValidationProblemDetails()
        {
            Status = StatusCodes.Status400BadRequest,
            Errors = GetValidationErrors(validationException.Errors)
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }

    private static List<string> GetValidationErrors(IEnumerable<FluentValidation.Results.ValidationFailure> errors)
    {
        var result = new List<string>(errors.Count());

        foreach (var error in errors)
        {
            result.Add(error.ErrorCode);
        }

        result.Sort(StringComparer.Ordinal);

        return result;
    }
}
