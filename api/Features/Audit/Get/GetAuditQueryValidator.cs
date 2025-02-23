using Domain.Exceptions;
using Features.Audit.BusinessRules;
using FluentValidation;

namespace Features.Audit.Get;

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
