using Domain;
using Features.AuditAction.BusinessRules;
using FluentValidation;

namespace Features.AuditAction.Delete;

public sealed class DeleteAuditActionCommandValidator : AbstractValidator<DeleteAuditActionCommand>
{
    public DeleteAuditActionCommandValidator(IAuditActionBusinessRules auditActionBusinessRules)
    {
        RuleFor(x => x.ActionId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.ActionIdIsRequired)
            .MustAsync(auditActionBusinessRules.AuditActionExists)
            .WithErrorCode(ErrorCodes.AuditActionDoesNotExist);
    }
}
