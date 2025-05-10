using Data.DbContext;
using Domain;
using Features.AuditActions.Dto;
using Features.Core.MappingService;
using Features.Core.MediatorService;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditActions.Save;

public sealed class SaveAuditActionCommandHandler : IRequestHandler<SaveAuditActionCommand, SaveAuditActionCommandResult>
{
    private readonly LeanAuditorContext _context;
    private readonly IMappingService _mapper;

    public SaveAuditActionCommandHandler(
        LeanAuditorContext context,
        IMappingService mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SaveAuditActionCommandResult> Handle(SaveAuditActionCommand command, CancellationToken cancellationToken)
    {
        // Find existing (presence already validated)
        var audit = await _context.Audits.FindAsync([command.AuditId], cancellationToken);

        // Create & add
        var auditAction = AuditAction.Create(
            auditActionId: command.AuditActionId,
            description: command.Description
        );
        audit!.AddActions(auditAction);
        await _context.SaveChangesAsync(cancellationToken);

        // Map
        var auditActionDto = _mapper.Map<AuditAction, AuditActionDto>(auditAction);
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
