using Api.Contracts.AuditAction.Requests;
using Core.ValidatorService;
using Domain;
using FluentValidation;

namespace Api.Contracts.Internal.Audit;

public class SaveAuditActionRequestValidator : CustomAbstractValidator<SaveAuditActionRequest>
{
    public SaveAuditActionRequestValidator()
    {
        RuleFor(x => x.ActionId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.ActionIdIsRequired);

        RuleFor(x => x.AuditId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.AuditIdIsRequired);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.DescriptionIsRequired);
    }
}
