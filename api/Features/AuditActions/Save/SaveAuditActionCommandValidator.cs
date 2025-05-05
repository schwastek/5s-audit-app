using Domain.Exceptions;
using Features.Audits.BusinessRules;
using Features.Core.ValidatorService;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditActions.Save;

public sealed class SaveAuditActionCommandValidator : AbstractValidator<SaveAuditActionCommand>
{
    private readonly IAuditBusinessRules _auditBusinessRules;

    public SaveAuditActionCommandValidator(IAuditBusinessRules auditBusinessRules)
    {
        _auditBusinessRules = auditBusinessRules;
    }

    public override async Task Validate(SaveAuditActionCommand instance, CancellationToken cancellationToken)
    {
        if (IsEmpty(instance.AuditId)) AddError(ErrorCodes.Audit.AuditIdIsRequired);
        if (IsEmpty(instance.AuditActionId)) AddError(ErrorCodes.AuditAction.AuditActionIdIsRequired);
        if (IsEmpty(instance.Description)) AddError(ErrorCodes.AuditAction.AuditActionDescriptionIsRequired);

        if (!IsValid) return;

        var auditExists = await _auditBusinessRules.AuditExists(instance.AuditId, cancellationToken);
        if (!auditExists) AddError(ErrorCodes.Audit.AuditDoesNotExist);
    }
}
