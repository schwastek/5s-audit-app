using Domain.Exceptions;
using Features.Accounts.BusinessRules;
using Infrastructure.ValidatorService;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Requests.Identity;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    private readonly IAccountBusinessRules _accountBusinessRules;

    public RegisterRequestValidator(IAccountBusinessRules accountBusinessRules)
    {
        _accountBusinessRules = accountBusinessRules;
    }

    public override async Task Validate(RegisterRequest instance, CancellationToken cancellationToken)
    {
        if (IsEmpty(instance.DisplayName)) AddError(ErrorCodes.Identity.IdentityDisplayNameIsRequired);
        if (IsEmpty(instance.Email)) AddError(ErrorCodes.Identity.IdentityEmailIsRequired);
        if (IsEmpty(instance.Username)) AddError(ErrorCodes.Identity.IdentityUsernameIsRequired);
        if (IsEmpty(instance.Password)) AddError(ErrorCodes.Identity.IdentityPasswordIsRequired);

        if (!IsValid) return;

        if (IsEmailAddress(instance.Email))
        {
            var isEmailAvailable = await _accountBusinessRules.IsEmailAvailable(instance.Email, cancellationToken);
            if (!isEmailAvailable) AddError(ErrorCodes.Identity.IdentityEmailIsAlreadyTaken);
        }
        else
        {
            AddError(ErrorCodes.Identity.IdentityEmailFormatIsNotValid);
        }

        var isUsernameAvailable = await _accountBusinessRules.IsUsernameAvailable(instance.Username, cancellationToken);
        if (!isUsernameAvailable) AddError(ErrorCodes.Identity.IdentityUsernameIsAlreadyTaken);

        // Error message: "Password is too weak. Password must be between 4 and 8 characters long,
        // including one lowercase letter, one uppercase letter, one number."
        var isPasswordStrong = _accountBusinessRules.PasswordMatchesStrengthCriteria(instance.Password);
        if (!isPasswordStrong) AddError(ErrorCodes.Identity.IdentityPasswordIsTooWeak);
    }
}
