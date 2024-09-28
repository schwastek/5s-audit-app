using Api.Contracts.Audit.Requests;
using Core.ValidatorService;
using Domain;
using FluentValidation;
using System;

namespace Api.Contracts.Internal.Audit;

public class GetAuditRequestValidator : CustomAbstractValidator<GetAuditRequest>
{
    public GetAuditRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorCodes.AuditIdIsRequired);
    }
}
