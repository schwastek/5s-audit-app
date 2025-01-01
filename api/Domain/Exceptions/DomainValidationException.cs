using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Domain.Exceptions;

public class DomainValidationException : Exception
{
    public ImmutableList<string> Errors { get; }

    public DomainValidationException(IEnumerable<string> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors.ToImmutableList();
    }

    public DomainValidationException()
    {
        Errors = [];
    }

    public DomainValidationException(string message)
        : base("One or more validation errors occurred.")
    {
        Errors = [message];
    }

    public DomainValidationException(string message, Exception inner)
        : base("One or more validation errors occurred.", inner)
    {
        Errors = [message];
    }
}
