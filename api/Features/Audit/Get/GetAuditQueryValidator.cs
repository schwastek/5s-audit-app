using Domain;
using Features.Audit.BusinessRules;
using FluentValidation;

namespace Features.Audit.Get;

public sealed class GetAuditQueryValidator : AbstractValidator<GetAuditQuery>
{
    public GetAuditQueryValidator(IAuditBusinessRules auditBusinessRules)
    {
        RuleFor(x => x.Id)
            .MustAsync(auditBusinessRules.AuditExists)
            .WithErrorCode(ErrorCodes.Audit.DoesNotExist);
    }
}
