using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Api.Exceptions
{
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

        private async Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = GetStatusCode(error);

            var responseBody = new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = error.Message,
            }.ToString();

            await context.Response.WriteAsync(responseBody);
        }

        private static int GetStatusCode(Exception error)
        {
            switch (error)
            {
                case NotFoundException:
                    return StatusCodes.Status404NotFound;
                default:
                    return StatusCodes.Status500InternalServerError;
            }
        }
    }
}
