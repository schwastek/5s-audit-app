using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Core.Exceptions;

public class ApplicationValidationException : Exception
{
    public ImmutableList<string> Errors { get; }

    public const string DefaultErrorMessage = "One or more validation errors occurred.";

    public ApplicationValidationException()
    {
        Errors = [];
    }

    // Only error.
    public ApplicationValidationException(string error)
        : base(DefaultErrorMessage)
    {
        Errors = [error];
    }

    public ApplicationValidationException(IEnumerable<string> error)
        : base(DefaultErrorMessage)
    {
        Errors = error.ToImmutableList();
    }

    // Message + error.
    public ApplicationValidationException(string message, string error)
        : base(message)
    {
        Errors = [error];
    }

    public ApplicationValidationException(string message, IEnumerable<string> errors)
        : base(message)
    {
        Errors = errors.ToImmutableList();
    }

    // Only error + inner Excepetion.
    public ApplicationValidationException(string error, Exception inner)
        : base(DefaultErrorMessage, inner)
    {
        Errors = [error];
    }

    public ApplicationValidationException(IEnumerable<string> errors, Exception inner)
        : base(DefaultErrorMessage, inner)
    {
        Errors = errors.ToImmutableList();
    }

    // Message + error + inner Exception.
    public ApplicationValidationException(string message, string error, Exception inner)
        : base(message, inner)
    {
        Errors = [error];
    }

    public ApplicationValidationException(string message, IEnumerable<string> errors, Exception inner)
        : base(message, inner)
    {
        Errors = errors.ToImmutableList();
    }
}
