using Domain.Exceptions;
using Features.Audits.BusinessRules;
using FluentValidation;

namespace Features.AuditActions.Save;

public sealed class SaveAuditActionCommandValidator : AbstractValidator<SaveAuditActionCommand>
{
    public SaveAuditActionCommandValidator(IAuditBusinessRules auditBusinessRules)
    {
        RuleFor(x => x.AuditId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Audit.AuditIdIsRequired)
            .DependentRules(() =>
            {
                RuleFor(x => x.AuditId)
                    .MustAsync(auditBusinessRules.AuditExists)
                    .WithErrorCode(ErrorCodes.Audit.DoesNotExist);
            });

        RuleFor(x => x.AuditActionId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.AuditAction.ActionIdIsRequired);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.AuditAction.DescriptionIsRequired);
    }
}
