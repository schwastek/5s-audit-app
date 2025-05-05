using Domain.Exceptions;
using Features.AuditActions.BusinessRules;
using Features.Core.ValidatorService;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditActions.Update;

public sealed class UpdateAuditActionCommandValidator : AbstractValidator<UpdateAuditActionCommand>
{
    private readonly IAuditActionBusinessRules _auditActionBusinessRules;

    public UpdateAuditActionCommandValidator(IAuditActionBusinessRules auditActionBusinessRules)
    {
        _auditActionBusinessRules = auditActionBusinessRules;
    }

    public override async Task Validate(UpdateAuditActionCommand instance, CancellationToken cancellationToken)
    {
        if (IsEmpty(instance.AuditActionId)) AddError(ErrorCodes.AuditAction.AuditActionIdIsRequired);
        if (IsEmpty(instance.Description)) AddError(ErrorCodes.AuditAction.AuditActionDescriptionIsRequired);

        if (!IsValid) return;

        var auditActionExists = await _auditActionBusinessRules.AuditActionExists(instance.AuditActionId, cancellationToken);
        if (!auditActionExists) AddError(ErrorCodes.AuditAction.AuditActionDoesNotExist);
    }
}
