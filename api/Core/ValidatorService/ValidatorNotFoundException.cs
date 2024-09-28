using System;

namespace Core.ValidatorService;

public sealed class ValidatorNotFoundException : Exception
{
    public ValidatorNotFoundException(Type target)
        : base($"No validator for '{target}' was found.")
    {
    }
}
