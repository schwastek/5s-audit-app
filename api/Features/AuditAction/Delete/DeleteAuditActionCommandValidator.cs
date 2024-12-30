using Domain;
using Features.AuditAction.BusinessRules;
using FluentValidation;

namespace Features.AuditAction.Delete;

public sealed class DeleteAuditActionCommandValidator : AbstractValidator<DeleteAuditActionCommand>
{
    public DeleteAuditActionCommandValidator(IAuditActionBusinessRules auditActionBusinessRules)
    {
        RuleFor(x => x.AuditActionId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.AuditAction.ActionIdIsRequired)
            .MustAsync(auditActionBusinessRules.AuditActionExists)
            .WithErrorCode(ErrorCodes.AuditAction.DoesNotExist);
    }
}
