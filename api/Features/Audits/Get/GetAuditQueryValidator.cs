using Domain.Exceptions;
using Features.Audits.BusinessRules;
using Features.Core.ValidatorService;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Audits.Get;

public sealed class GetAuditQueryValidator : AbstractValidator<GetAuditQuery>
{
    private readonly IAuditBusinessRules _auditBusinessRules;

    public GetAuditQueryValidator(IAuditBusinessRules auditBusinessRules)
    {
        _auditBusinessRules = auditBusinessRules;
    }

    public override async Task Validate(GetAuditQuery instance, CancellationToken cancellationToken)
    {
        if (IsEmpty(instance.Id)) AddError(ErrorCodes.Audit.AuditIdIsRequired);

        if (!IsValid) return;

        var auditExists = await _auditBusinessRules.AuditExists(instance.Id, cancellationToken);
        if (!auditExists) AddError(ErrorCodes.Audit.AuditDoesNotExist);
    }
}
