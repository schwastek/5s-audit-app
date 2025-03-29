using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Exceptions;

/// <summary>
/// A custom implementation of <see cref="IProblemDetailsWriter"/> that follows the same logic
/// as <see cref="DefaultProblemDetailsWriter"/>, but does not reject requests based on the
/// `Accept` HTTP header. Even if the request has an invalid `Accept` header, it will still be processed.
/// This means that errors can still be thrown. This <see cref="CustomProblemDetailsWriter"/> ensures
/// that a proper <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails"/> error response is written for those cases.
/// </summary>
/// <remarks>
/// Note that the <see cref="Microsoft.AspNetCore.Mvc.ProducesAttribute"/> does not reject requests based on the `Accept` header.
/// Therefore, even if this attribute is applied to a controller action, it will not prevent requests with invalid `Accept` headers from reaching the action.
/// In contrast, the <see cref="Microsoft.AspNetCore.Mvc.ConsumesAttribute"/> ensures that requests with an invalid `Content-Type` header are rejected by the API.
/// </remarks>
public class CustomProblemDetailsWriter : IProblemDetailsWriter
{
    private readonly ProblemDetailsOptions _options;
    private readonly JsonSerializerOptions _serializerOptions;

    public CustomProblemDetailsWriter(IOptions<ProblemDetailsOptions> options, IOptions<JsonOptions> jsonOptions)
    {
        _options = options.Value;
        _serializerOptions = jsonOptions.Value.SerializerOptions;
    }

    public bool CanWrite(ProblemDetailsContext context)
    {
        // Decide when to use this writer (true = always).
        return true;
    }

    public ValueTask WriteAsync(ProblemDetailsContext context)
    {
        var httpContext = context.HttpContext;
        _options.CustomizeProblemDetails?.Invoke(context);

        var problemDetailsType = context.ProblemDetails.GetType();

        return new ValueTask(httpContext.Response.WriteAsJsonAsync(
                        context.ProblemDetails,
                         _serializerOptions.GetTypeInfo(problemDetailsType),
                        contentType: MediaTypeNames.Application.ProblemJson));
    }
}
