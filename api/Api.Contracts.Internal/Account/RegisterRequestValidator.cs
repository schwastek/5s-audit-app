using Api.Contracts.Identity.Requests;
using Domain;
using Features.Account.BusinessRules;
using FluentValidation;

namespace Api.Contracts.Internal.Account;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(IAccountBusinessRules accountBusinessRules)
    {
        RuleFor(r => r.DisplayName)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.DisplayNameIsRequired);

        RuleFor(r => r.Email)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.EmailIsRequired)
            .EmailAddress()
            .WithErrorCode(ErrorCodes.EmailFormatIsNotValid)
            .MustAsync(accountBusinessRules.IsEmailAvailable)
            .WithErrorCode(ErrorCodes.EmailIsAlreadyTaken);

        RuleFor(r => r.Username)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.UsernameIsRequired)
            .MustAsync(accountBusinessRules.IsUsernameAvailable)
            .WithErrorCode(ErrorCodes.UsernameIsAlreadyTaken);

        RuleFor(r => r.Password)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.PasswordIsRequired)
            .Must(accountBusinessRules.PasswordMatchesStrengthCriteria)
            // Error message: "Password is too weak. Password must be between 4 and 8 characters long,
            // including one lowercase letter, one uppercase letter, one number."
            .WithErrorCode(ErrorCodes.PasswordIsTooWeak);
    }
}
