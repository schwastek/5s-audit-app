using Api.Contracts.AuditAction.Requests;
using Core.ValidatorService;
using Domain;
using FluentValidation;
using System;

namespace Api.Contracts.Internal.Audit;

public class SaveAuditActionRequestValidator : CustomAbstractValidator<SaveAuditActionRequest>
{
    public SaveAuditActionRequestValidator()
    {
        RuleFor(x => x.ActionId)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorCodes.ActionIdIsRequired);

        RuleFor(x => x.AuditId)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorCodes.AuditIdIsRequired);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.DescriptionIsRequired);
    }
}
