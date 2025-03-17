using Features.Accounts.BusinessRules;
using Domain.Exceptions;
using FluentValidation;

namespace Api.Requests.Identity;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(IAccountBusinessRules accountBusinessRules)
    {
        RuleFor(r => r.DisplayName)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Identity.DisplayNameIsRequired);

        RuleFor(r => r.Email)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Identity.EmailIsRequired)
            .EmailAddress()
            .WithErrorCode(ErrorCodes.Identity.EmailFormatIsNotValid)
            .MustAsync(accountBusinessRules.IsEmailAvailable)
            .WithErrorCode(ErrorCodes.Identity.EmailIsAlreadyTaken);

        RuleFor(r => r.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Identity.UsernameIsRequired)
            .MustAsync(accountBusinessRules.IsUsernameAvailable)
            .WithErrorCode(ErrorCodes.Identity.UsernameIsAlreadyTaken);

        RuleFor(r => r.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Identity.PasswordIsRequired)
            .Must(accountBusinessRules.PasswordMatchesStrengthCriteria)
            // Error message: "Password is too weak. Password must be between 4 and 8 characters long,
            // including one lowercase letter, one uppercase letter, one number."
            .WithErrorCode(ErrorCodes.Identity.PasswordIsTooWeak);
    }
}
