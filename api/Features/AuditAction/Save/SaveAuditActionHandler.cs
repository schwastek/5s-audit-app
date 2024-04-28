using Api.Mappers.MappingService;
using Core.Exceptions;
using Data.DbContext;
using Features.Audit.BusinessRules;
using Features.AuditAction.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditAction.Save;

public sealed class SaveAuditActionHandler : IRequestHandler<SaveAuditActionCommand, SaveAuditActionCommandResult>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;
    private readonly IAuditBusinessRules auditBusinessRules;

    public SaveAuditActionHandler(
        LeanAuditorContext context,
        IMappingService mapper,
        IAuditBusinessRules auditBusinessRules
    )
    {
        this.context = context;
        this.mapper = mapper;
        this.auditBusinessRules = auditBusinessRules;
    }

    public async Task<SaveAuditActionCommandResult> Handle(SaveAuditActionCommand command, CancellationToken cancellationToken)
    {
        // TODO: Move to validator.
        var auditExists = await auditBusinessRules.AuditExists(command.AuditId, cancellationToken);
        if (!auditExists)
        {
            throw new NotFoundException($"Audit with ID {command.AuditId} does not exist.");
        }

        // Add to DB
        var auditAction = new Domain.AuditAction()
        {
            AuditActionId = command.ActionId,
            AuditId = command.AuditId,
            Description = command.Description,
            IsComplete = command.IsComplete
        };
        context.AuditActions.Add(auditAction);
        await context.SaveChangesAsync(cancellationToken);

        // Map
        AuditActionDto auditActionDto = mapper.Map<Domain.AuditAction, AuditActionDto>(auditAction);

        var result = new SaveAuditActionCommandResult()
        {
            AuditAction = auditActionDto
        };

        return result;
    }
}
