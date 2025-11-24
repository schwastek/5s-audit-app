using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Exceptions.Handlers;

public class ConcurrencyExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public ConcurrencyExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is not DbUpdateConcurrencyException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

        var problemDetails = new ProblemDetails()
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            Title = "ConcurrencyConflict",
            Detail = "The record has been modified by another user."
        };

        var result = await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });

        return result;
    }
}
