using Domain;
using Features.AuditAction.BusinessRules;
using FluentValidation;

namespace Features.AuditAction.Update;

public sealed class UpdateAuditActionCommandValidator : AbstractValidator<UpdateAuditActionCommand>
{
    public UpdateAuditActionCommandValidator(IAuditActionBusinessRules auditActionBusinessRules)
    {
        RuleFor(x => x.AuditActionId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.AuditAction.ActionIdIsRequired)
            .DependentRules(() =>
            {
                RuleFor(x => x.AuditActionId)
                    .MustAsync(auditActionBusinessRules.AuditActionExists)
                    .WithErrorCode(ErrorCodes.AuditAction.DoesNotExist);
            });

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.AuditAction.DescriptionIsRequired);
    }
}
