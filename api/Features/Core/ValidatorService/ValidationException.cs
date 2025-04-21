using System;
using System.Collections.Generic;

namespace Features.Core.ValidatorService;

public class ValidationException : Exception
{
    public IEnumerable<string> Errors { get; private set; }

    public const string DefaultErrorMessage = "One or more validation errors occurred.";

    public ValidationException()
    {
        Errors = [];
    }

    public ValidationException(string error)
        : base(DefaultErrorMessage)
    {
        Errors = [error];
    }

    public ValidationException(string error, Exception inner)
        : base(DefaultErrorMessage, inner)
    {
        Errors = [error];
    }

    public ValidationException(IEnumerable<string> errors)
        : base(DefaultErrorMessage)
    {
        Errors = errors;
    }

    public ValidationException(IEnumerable<string> errors, Exception inner)
        : base(DefaultErrorMessage, inner)
    {
        Errors = errors;
    }
}
