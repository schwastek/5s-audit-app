using FluentValidation;
using FluentValidation.Results;

namespace Features.Core.ValidatorService;

public class CustomAbstractValidator<T> : FluentValidation.AbstractValidator<T>
{
    protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
    {
        // If we send a request with an empty request body.
        if (context.InstanceToValidate is null)
        {
            result.Errors.Add(new ValidationFailure("", "The request is empty or invalid."));

            return false;
        }

        return true;
    }
}
