using Api.Contracts.AuditAction.Requests;
using Core.ValidatorService;
using Domain;
using FluentValidation;

namespace Api.Contracts.Internal.AuditAction;

public class UpdateAuditActionRequestValidator : CustomAbstractValidator<UpdateAuditActionRequest>
{
    public UpdateAuditActionRequestValidator()
    {
        RuleFor(x => x.ActionId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.ActionIdIsRequired);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.DescriptionIsRequired);
    }
}
