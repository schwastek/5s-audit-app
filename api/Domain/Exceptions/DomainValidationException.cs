using System;
using System.Collections.Generic;

namespace Domain.Exceptions;

public class DomainValidationException : Exception
{
    public IEnumerable<string> Errors { get; private set; }

    public const string DefaultErrorMessage = "One or more validation errors occurred.";

    public DomainValidationException(IEnumerable<string> errors)
        : base(DefaultErrorMessage)
    {
        Errors = errors;
    }

    public DomainValidationException()
    {
        Errors = [];
    }

    public DomainValidationException(string error)
        : base(DefaultErrorMessage)
    {
        Errors = [error];
    }

    public DomainValidationException(string error, Exception inner)
        : base(DefaultErrorMessage, inner)
    {
        Errors = [error];
    }
}
