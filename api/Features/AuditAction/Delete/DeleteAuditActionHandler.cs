using Data.DbContext;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditAction.Delete;

public sealed class DeleteAuditActionHandler : IRequestHandler<DeleteAuditActionCommand>
{
    private readonly LeanAuditorContext context;

    public DeleteAuditActionHandler(LeanAuditorContext context)
    {
        this.context = context;
    }

    public async Task Handle(DeleteAuditActionCommand command, CancellationToken cancellationToken)
    {
        // Find existing (presence already validated)
        var auditAction = await context.AuditActions.FindAsync([command.ActionId], cancellationToken);
        context.Remove(auditAction!);
        await context.SaveChangesAsync(cancellationToken);
    }
}
