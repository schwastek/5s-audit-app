using System;

namespace Api.Mappers.ValidatorService;

public sealed class ValidatorNotFoundException : Exception
{
    public ValidatorNotFoundException(Type instance)
        : base($"No Validator for '{instance}' was found.")
    {
    }
}
