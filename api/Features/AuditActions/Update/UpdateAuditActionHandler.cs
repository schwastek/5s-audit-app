using Data.DbContext;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditActions.Update;

public sealed class UpdateAuditActionHandler : IRequestHandler<UpdateAuditActionCommand>
{
    private readonly LeanAuditorContext context;

    public UpdateAuditActionHandler(LeanAuditorContext context)
    {
        this.context = context;
    }

    public async Task Handle(UpdateAuditActionCommand command, CancellationToken cancellationToken)
    {
        // Find existing (presence already validated)
        var auditAction = await context.AuditActions.FindAsync([command.AuditActionId], cancellationToken);

        // Update
        auditAction!.SetCompletionStatus(command.IsComplete);
        auditAction!.ChangeDescription(command.Description);

        await context.SaveChangesAsync(cancellationToken);
    }
}
