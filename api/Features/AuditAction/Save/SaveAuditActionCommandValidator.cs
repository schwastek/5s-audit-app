using Domain;
using Features.Audit.BusinessRules;
using Features.AuditAction.BusinessRules;
using FluentValidation;

namespace Features.AuditAction.Save;

public sealed class SaveAuditActionCommandValidator : AbstractValidator<SaveAuditActionCommand>
{
    public SaveAuditActionCommandValidator(IAuditBusinessRules auditBusinessRules, IAuditActionBusinessRules auditActionBusinessRules)
    {
        RuleFor(x => x.AuditId)
            .MustAsync(auditBusinessRules.AuditExists)
            .WithErrorCode(ErrorCodes.AuditDoesNotExist);

        RuleFor(x => x.Description)
            .MaximumLength(auditActionBusinessRules.DescriptionMaxLength)
            .WithErrorCode(ErrorCodes.AuditActionDescriptionIsTooLong);
    }
}
