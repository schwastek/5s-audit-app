using Api.Contracts.Identity.Requests;
using Domain;
using FluentValidation;

namespace Api.Contracts.Internal.Account;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Identity.EmailIsRequired);

        RuleFor(r => r.Password)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Identity.PasswordIsRequired);
    }
}
