using Api.Data;
using Api.Core.Domain;
using Api.Mappers;
using Api.Models;
using Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Handlers;

public sealed class CreateAuditHandler : IRequestHandler<CreateAuditCommand, AuditDto>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;

    public CreateAuditHandler(LeanAuditorContext context, IMappingService mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<AuditDto> Handle(CreateAuditCommand request, CancellationToken cancellationToken)
    {
        Audit audit = mapper.Map<AuditForCreationDto, Audit>(request.Audit);

        context.Audits.Add(audit);
        await context.SaveChangesAsync();

        // Explicitly load question for each answer.
        // Otherwise we would only have the question ID and no question text.
        // Passed request doesn't include question text.
        await context.Entry(audit)
            .Collection(a => a.Answers)
            .Query()
            .Include(a => a.Question)
            .LoadAsync();

        AuditDto auditDto = mapper.Map<Audit, AuditDto>(audit);

        return auditDto;
    }
}
