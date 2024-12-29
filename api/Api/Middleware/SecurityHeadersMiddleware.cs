using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Api.Middleware;

/// <summary>
/// This middleware adds security headers to HTTP responses for API that does not return HTML.
/// 
/// This middleware implementation is based on the recommendations from:
/// <see href="https://cheatsheetseries.owasp.org/cheatsheets/REST_Security_Cheat_Sheet.html#security-headers">OWASP REST Security Headers recommendations</see>.
/// <see href="https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders/blob/62e048ab00245203d35d157ed4c1011aa1880211/src/NetEscapades.AspNetCore.SecurityHeaders/HeaderPolicyCollectionExtensions.cs">NetEscapades.AspNetCore.SecurityHeaders</see>.
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            // Header used to specify whether a response can be framed in a <frame>, <iframe>, <embed> or <object> element.
            // For an API response, there is no requirement to be framed in any of those elements.
            // Providing DENY prevents any domain from framing the response returned by the API call. 
            context.Response.Headers["X-Frame-Options"] = "DENY";

            // Disable MIME sniffing. Header to instruct a browser to always use the MIME type
            // that is declared in the Content-Type header rather than trying to determine the MIME type based on the file's content.
            // This header with a nosniff value prevents browsers from performing MIME sniffing, and inappropriately interpreting responses as HTML.
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";

            // Content-Security-Policy:
            // 1. frame-ancestors 'none':
            //      - Prevents the API response from being embedded in any iframe or frame, protecting against drag-and-drop style clickjacking attacks.
            context.Response.Headers["Content-Security-Policy"] = "frame-ancestors 'none'";

            return Task.CompletedTask;
        });

        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}
