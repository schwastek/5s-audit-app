using Api.Contracts.AuditAction.Requests;
using Core.ValidatorService;
using Domain;
using FluentValidation;

namespace Api.Contracts.Internal.AuditAction;

public class SaveAuditActionRequestValidator : CustomAbstractValidator<SaveAuditActionRequest>
{
    public SaveAuditActionRequestValidator()
    {
        RuleFor(x => x.ActionId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.AuditAction.ActionIdIsRequired);

        RuleFor(x => x.AuditId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Audit.AuditIdIsRequired);
    }
}
