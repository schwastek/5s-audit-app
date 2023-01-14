using Api.DbContexts;
using Api.Domain;
using Api.Exceptions;
using Api.Mappers;
using Api.Models;
using Api.Queries;
using Api.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Handlers;

public sealed class CreateAuditActionHandler : IRequestHandler<CreateAuditActionCommand, AuditActionDto>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;
    private readonly AuditService auditService;

    public CreateAuditActionHandler(LeanAuditorContext context, IMappingService mapper, 
        AuditService auditService)
    {
        this.context = context;
        this.mapper = mapper;
        this.auditService = auditService;
    }

    public async Task<AuditActionDto> Handle(CreateAuditActionCommand request, CancellationToken cancellationToken)
    {
        if (!auditService.AuditExists(request.AuditAction.AuditId))
        {
            throw new AuditNotFoundException(request.AuditAction.AuditId);
        }

        // Map
        AuditAction auditAction = mapper.Map<AuditActionForCreationDto, AuditAction>(request.AuditAction);

        // Add to DB
        context.AuditActions.Add(auditAction);
        await context.SaveChangesAsync();

        // Map
        AuditActionDto auditActionDto = mapper.Map<AuditAction, AuditActionDto>(auditAction);

        return auditActionDto;
    }
}
