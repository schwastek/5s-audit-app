using Domain.Exceptions;
using Infrastructure.ValidatorService;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Audits.Save;

public sealed class SaveAuditCommandValidator : AbstractValidator<SaveAuditCommand>
{
    public override Task Validate(SaveAuditCommand instance, CancellationToken cancellationToken)
    {
        if (IsEmpty(instance.AuditId)) AddError(ErrorCodes.Audit.AuditIdIsRequired);
        if (IsEmpty(instance.Author)) AddError(ErrorCodes.Audit.AuditAuthorIsRequired);
        if (IsEmpty(instance.Area)) AddError(ErrorCodes.Audit.AuditAreaIsRequired);
        if (IsEmpty(instance.StartDate)) AddError(ErrorCodes.Audit.AuditStartDateIsRequired);
        if (IsEmpty(instance.EndDate)) AddError(ErrorCodes.Audit.AuditEndDateIsRequired);
        if (IsEmpty(instance.Answers)) AddError(ErrorCodes.Audit.AuditAnswersIsRequired);

        return Task.CompletedTask;
    }
}
