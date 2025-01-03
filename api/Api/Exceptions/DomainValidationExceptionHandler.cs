﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Exceptions;

public class DomainValidationExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService problemDetailsService;

    public DomainValidationExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        this.problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not Domain.Exceptions.DomainValidationException domainValidationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var problemDetails = new CustomValidationProblemDetails()
        {
            Errors = domainValidationException.Errors
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}
