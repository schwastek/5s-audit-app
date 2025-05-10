using Data.DbContext;
using Features.Core.MediatorService;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditActions.Update;

public sealed class UpdateAuditActionHandler : IRequestHandler<UpdateAuditActionCommand, Unit>
{
    private readonly LeanAuditorContext _context;

    public UpdateAuditActionHandler(LeanAuditorContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateAuditActionCommand command, CancellationToken cancellationToken)
    {
        // Find existing (presence already validated)
        var auditAction = await _context.AuditActions.FindAsync([command.AuditActionId], cancellationToken);

        // Update
        auditAction!.SetCompletionStatus(command.IsComplete);
        auditAction!.ChangeDescription(command.Description);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
