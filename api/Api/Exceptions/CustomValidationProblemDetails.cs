using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Exceptions;

public class CustomValidationProblemDetails : ProblemDetails
{
    public ICollection<string> Errors { get; set; } = new List<string>();

    public CustomValidationProblemDetails()
    {
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1";
        Title = "One or more validation errors occurred";
        Detail = "See the 'errors' property for a list of specific error codes";
    }
}
