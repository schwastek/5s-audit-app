using Data.DbContext;
using Data.Extensions;
using Infrastructure.MediatorService;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditActions.Update;

public sealed class UpdateAuditActionHandler : IRequestHandler<UpdateAuditActionCommand, UpdateAuditActionCommandResult>
{
    private readonly LeanAuditorContext _context;

    public UpdateAuditActionHandler(LeanAuditorContext context)
    {
        _context = context;
    }

    public async Task<UpdateAuditActionCommandResult> Handle(UpdateAuditActionCommand command, CancellationToken cancellationToken)
    {
        // Find existing (presence already validated)
        var auditAction = await _context.AuditActions.FindAsync([command.AuditActionId], cancellationToken);

        // Set concurrency token
        _context.SetConcurrencyToken(auditAction!, command.LastVersion);

        // Update
        auditAction!.SetCompletionStatus(command.IsComplete);
        auditAction!.ChangeDescription(command.Description);

        await _context.SaveChangesAsync(cancellationToken);

        var result = new UpdateAuditActionCommandResult()
        {
            AuditActionId = auditAction.AuditActionId,
            Description = auditAction.Description,
            IsComplete = auditAction.IsComplete,
            LastVersion = auditAction.Version
        };

        return result;
    }
}
