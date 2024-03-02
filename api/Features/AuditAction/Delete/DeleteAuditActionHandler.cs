using Core.Exceptions;
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
        var auditAction = await context.AuditActions.FindAsync(new object?[] { command.ActionId }, cancellationToken: cancellationToken);

        if (auditAction is null)
        {
            throw new NotFoundException($"Action with ID {command.ActionId} does not exist.");
        }

        context.Remove(auditAction);
        await context.SaveChangesAsync(cancellationToken);
    }
}
