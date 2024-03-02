using Core.Exceptions;
using Data.DbContext;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditAction.Update;

public sealed class UpdateAuditActionHandler : IRequestHandler<UpdateAuditActionCommand>
{
    private readonly LeanAuditorContext context;

    public UpdateAuditActionHandler(LeanAuditorContext context)
    {
        this.context = context;
    }

    public async Task Handle(UpdateAuditActionCommand command, CancellationToken cancellationToken)
    {
        var auditAction = await context.AuditActions.FindAsync(new object?[] { command.ActionId }, cancellationToken: cancellationToken);

        if (auditAction is null)
        {
            throw new NotFoundException($"Action with ID {command.ActionId} does not exist.");
        }

        auditAction.Description = command.Description ?? auditAction.Description;
        auditAction.IsComplete = command.IsComplete;

        await context.SaveChangesAsync(cancellationToken);
    }
}
