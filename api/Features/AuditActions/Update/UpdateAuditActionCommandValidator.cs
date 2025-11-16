using Domain.Constants;
using Domain.Exceptions;
using Features.AuditActions.BusinessRules;
using Infrastructure.ValidatorService;
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

        if (instance.Description.Length > AuditActionConstants.DescriptionMaxLength)
        {
            AddError(ErrorCodes.AuditAction.AuditActionDescriptionIsTooLong);
        }

        var auditActionExists = await _auditActionBusinessRules.AuditActionExists(instance.AuditActionId, cancellationToken);
        if (!auditActionExists) AddError(ErrorCodes.AuditAction.AuditActionDoesNotExist);
    }
}
