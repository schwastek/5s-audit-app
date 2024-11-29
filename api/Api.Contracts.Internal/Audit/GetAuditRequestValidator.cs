using Api.Contracts.Audit.Requests;
using Core.ValidatorService;
using Domain;
using FluentValidation;

namespace Api.Contracts.Internal.Audit;

public class GetAuditRequestValidator : CustomAbstractValidator<GetAuditRequest>
{
    public GetAuditRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Audit.AuditIdIsRequired);
    }
}
