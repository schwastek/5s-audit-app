using Features.Audits.BusinessRules;
using Domain.Exceptions;
using FluentValidation;

namespace Features.Audits.Get;

public sealed class GetAuditQueryValidator : AbstractValidator<GetAuditQuery>
{
    public GetAuditQueryValidator(IAuditBusinessRules auditBusinessRules)
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Audit.AuditIdIsRequired)
            .MustAsync(auditBusinessRules.AuditExists)
            .WithErrorCode(ErrorCodes.Audit.DoesNotExist);
    }
}
