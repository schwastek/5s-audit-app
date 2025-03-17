using Features.AuditActions.BusinessRules;
using Domain.Exceptions;
using FluentValidation;

namespace Features.AuditActions.Delete;

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
