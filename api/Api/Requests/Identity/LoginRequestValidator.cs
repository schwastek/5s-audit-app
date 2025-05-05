using Domain.Exceptions;
using Features.Core.ValidatorService;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Requests.Identity;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public override Task Validate(LoginRequest instance, CancellationToken cancellationToken)
    {
        if (IsEmpty(instance.Email)) AddError(ErrorCodes.Identity.IdentityEmailIsRequired);
        if (IsEmpty(instance.Password)) AddError(ErrorCodes.Identity.IdentityPasswordIsRequired);

        return Task.CompletedTask;
    }
}
