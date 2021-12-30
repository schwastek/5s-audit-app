using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Api.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Protect against MIME sniffing
            context.Response.Headers.Add("X-Content-Type-Options", new StringValues("nosniff"));

            // The Referer header will be omitted entirely. No referrer information is sent along with requests.
            context.Response.Headers.Add("Referrer-Policy", new StringValues("no-referrer"));

            // The page cannot be displayed in a frame
            context.Response.Headers.Add("X-Frame-Options", new StringValues("DENY"));

            // Permit cross-domain requests from Flash and PDF documents
            context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", new StringValues("none"));

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
