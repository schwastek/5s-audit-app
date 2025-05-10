using Data.DbContext;
using Features.Core.MediatorService;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditActions.Delete;

public sealed class DeleteAuditActionHandler : IRequestHandler<DeleteAuditActionCommand, Unit>
{
    private readonly LeanAuditorContext _context;

    public DeleteAuditActionHandler(LeanAuditorContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteAuditActionCommand command, CancellationToken cancellationToken)
    {
        // Find existing (presence already validated)
        var auditAction = await _context.AuditActions.FindAsync([command.AuditActionId], cancellationToken);
        _context.Remove(auditAction!);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
