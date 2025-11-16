using Domain.Exceptions;
using Features.AuditActions.BusinessRules;
using Infrastructure.ValidatorService;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditActions.Delete;

public sealed class DeleteAuditActionCommandValidator : AbstractValidator<DeleteAuditActionCommand>
{
    private readonly IAuditActionBusinessRules _auditActionBusinessRules;

    public DeleteAuditActionCommandValidator(IAuditActionBusinessRules auditActionBusinessRules)
    {
        _auditActionBusinessRules = auditActionBusinessRules;
    }

    public override async Task Validate(DeleteAuditActionCommand instance, CancellationToken cancellationToken)
    {
        if (IsEmpty(instance.AuditActionId)) AddError(ErrorCodes.AuditAction.AuditActionIdIsRequired);

        if (!IsValid) return;

        var auditActionExists = await _auditActionBusinessRules.AuditActionExists(instance.AuditActionId, cancellationToken);
        if (!auditActionExists) AddError(ErrorCodes.AuditAction.AuditActionDoesNotExist);
    }
}
