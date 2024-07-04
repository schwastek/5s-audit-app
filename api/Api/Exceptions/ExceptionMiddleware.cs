using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Api.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            await HandleExceptionAsync(context, error);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception error)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = GetStatusCode(error);

        // TODO: Display better error message for ValidationException from FluentValidation.
        var responseBody = new ErrorDetails(context.Response.StatusCode, error.Message).ToString();

        await context.Response.WriteAsync(responseBody);
    }

    private static int GetStatusCode(Exception error)
    {
        switch (error)
        {
            case NotFoundException:
                return StatusCodes.Status404NotFound;
            case BadRequestException:
                return StatusCodes.Status400BadRequest;
            case System.ComponentModel.DataAnnotations.ValidationException:
                return StatusCodes.Status417ExpectationFailed;
            case FluentValidation.ValidationException:
                return StatusCodes.Status417ExpectationFailed;
            default:
                return StatusCodes.Status500InternalServerError;
        }
    }
}
