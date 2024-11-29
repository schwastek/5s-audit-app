using Domain;
using Features.Audit.BusinessRules;
using FluentValidation;

namespace Features.AuditAction.Save;

public sealed class SaveAuditActionCommandValidator : AbstractValidator<SaveAuditActionCommand>
{
    public SaveAuditActionCommandValidator(IAuditBusinessRules auditBusinessRules)
    {
        RuleFor(x => x.AuditId)
            .MustAsync(auditBusinessRules.AuditExists)
            .WithErrorCode(ErrorCodes.Audit.DoesNotExist);
    }
}
