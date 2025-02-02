using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Exceptions;

/// <summary>
/// A custom implementation of <see cref="ProblemDetails"/> that utilizes the <see cref="ProblemDetails.Extensions"/>
/// property instead of defining custom properties. This ensures compatibility with
/// <see cref="Microsoft.AspNetCore.Mvc.Infrastructure.DefaultApiProblemDetailsWriter"/>,
/// which expects and includes the <see cref="ProblemDetails.Extensions"/> in the response.
/// </summary>
/// <remarks>
/// The <see cref="Microsoft.AspNetCore.Mvc.Infrastructure.DefaultApiProblemDetailsWriter"/>
/// writes the API error response if no other <see cref="Microsoft.AspNetCore.Http.IProblemDetailsWriter"/>
/// handles the exception first (based on the <see cref="Microsoft.AspNetCore.Http.IProblemDetailsWriter.CanWrite"/> method).
/// </remarks>
public class CustomValidationProblemDetails : ProblemDetails
{
    public ICollection<string> Errors
    {
        get => Extensions.TryGetValue("errors", out var value) && value is ICollection<string> errors
            ? errors
            : [];
        set => Extensions["errors"] = value;
    }

    public CustomValidationProblemDetails()
    {
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1";
        Title = "One or more validation errors occurred";
        Detail = "See the 'errors' property for a list of specific error codes";
    }
}
