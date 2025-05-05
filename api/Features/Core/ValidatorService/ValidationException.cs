using System;
using System.Collections.Generic;

namespace Features.Core.ValidatorService;

public class ValidationException : Exception
{
    public IEnumerable<ValidationError> Errors { get; private set; }

    public const string DefaultErrorMessage = "One or more validation errors occurred.";

    public ValidationException()
    {
        Errors = [];
    }

    public ValidationException(ValidationError error)
        : base(DefaultErrorMessage)
    {
        Errors = [error];
    }

    public ValidationException(ValidationError error, Exception inner)
        : base(DefaultErrorMessage, inner)
    {
        Errors = [error];
    }

    public ValidationException(IEnumerable<ValidationError> errors)
        : base(DefaultErrorMessage)
    {
        Errors = errors;
    }

    public ValidationException(IEnumerable<ValidationError> errors, Exception inner)
        : base(DefaultErrorMessage, inner)
    {
        Errors = errors;
    }
}
