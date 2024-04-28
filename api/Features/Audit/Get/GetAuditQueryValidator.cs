using Domain;
using Features.Audit.BusinessRules;
using FluentValidation;

namespace Features.Audit.Get;

public sealed class GetAuditQueryValidator : AbstractValidator<GetAuditQuery>
{
    public GetAuditQueryValidator(IAuditBusinessRules auditBusinessRules)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.AuditIdIsRequired)
            .MustAsync(auditBusinessRules.AuditExists)
            .WithErrorCode(ErrorCodes.AuditDoesNotExist)
            // TODO: Add localized message (RESX).
            .WithMessage(x => string.Format("Audit with ID '{0}' does not exist.", x.Id))
            .WithState(x => x.Id);
    }
}
