using Domain;
using Features.AuditAction.BusinessRules;
using FluentValidation;

namespace Features.AuditAction.Update;

public sealed class UpdateAuditActionCommandValidator : AbstractValidator<UpdateAuditActionCommand>
{
    public UpdateAuditActionCommandValidator(IAuditActionBusinessRules auditActionBusinessRules)
    {
        RuleFor(x => x.ActionId)
            .MustAsync(auditActionBusinessRules.AuditActionExists)
            .WithErrorCode(ErrorCodes.AuditActionDoesNotExist);

        RuleFor(x => x.Description)
            .MaximumLength(auditActionBusinessRules.DescriptionMaxLength)
            .WithErrorCode(ErrorCodes.AuditActionDescriptionIsTooLong);
    }
}
