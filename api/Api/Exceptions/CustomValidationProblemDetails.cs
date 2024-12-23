using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Exceptions;

public class CustomValidationProblemDetails : ProblemDetails
{
    public ICollection<string> Errors { get; set; } = new List<string>();
}
