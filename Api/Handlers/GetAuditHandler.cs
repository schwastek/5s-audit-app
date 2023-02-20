using Api.DbContexts;
using Api.Core.Domain;
using Api.Exceptions;
using Api.Mappers;
using Api.Models;
using Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Handlers;

public sealed class GetAuditHandler : IRequestHandler<GetAuditQuery, AuditDto>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;

    public GetAuditHandler(LeanAuditorContext context, IMappingService mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<AuditDto> Handle(GetAuditQuery request, CancellationToken cancellationToken)
    {
        var audit = await context.Audits
            .Include(audit => audit.Actions)
            .Include(audit => audit.Answers)
            .ThenInclude(answer => answer.Question)
            .AsNoTracking()
            .SingleOrDefaultAsync(audit => audit.AuditId.Equals(request.AuditId));

        if (audit == null)
        {
            throw new AuditNotFoundException(request.AuditId);
        }

        var auditDto = mapper.Map<Audit, AuditDto>(audit);

        return auditDto;
    }
}
