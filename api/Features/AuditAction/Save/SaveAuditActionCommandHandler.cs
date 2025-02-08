using Core.MappingService;
using Data.DbContext;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditAction.Save;

public sealed class SaveAuditActionCommandHandler : IRequestHandler<SaveAuditActionCommand, SaveAuditActionCommandResult>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;

    public SaveAuditActionCommandHandler(
        LeanAuditorContext context,
        IMappingService mapper
    )
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<SaveAuditActionCommandResult> Handle(SaveAuditActionCommand command, CancellationToken cancellationToken)
    {
        // Find existing (presence already validated)
        var audit = await context.Audits.FindAsync([command.AuditId], cancellationToken);

        // Create & add
        var auditAction = Domain.AuditAction.Create(
            auditActionId: command.AuditActionId,
            description: command.Description
        );
        audit!.AddActions(auditAction);
        await context.SaveChangesAsync(cancellationToken);

        // Map
        var auditActionDto = mapper.Map<Domain.AuditAction, Features.AuditAction.Dto.AuditActionDto>(auditAction);
        var result = new SaveAuditActionCommandResult()
        {
            AuditActionId = auditActionDto.AuditActionId,
            AuditId = auditActionDto.AuditId,
            Description = auditActionDto.Description,
            IsComplete = auditActionDto.IsComplete
        };

        return result;
    }
}
