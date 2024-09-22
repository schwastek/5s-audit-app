using Core.Exceptions;
using Core.MappingService;
using Data.DbContext;
using Features.Audit.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Audit.Get;

public sealed class GetAuditQueryHandler : IRequestHandler<GetAuditQuery, GetAuditQueryResult>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;

    public GetAuditQueryHandler(LeanAuditorContext context, IMappingService mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<GetAuditQueryResult> Handle(GetAuditQuery query, CancellationToken cancellationToken)
    {
        var audit = await context.Audits
            .AsNoTracking()
            .Include(audit => audit.Actions)
            .Include(audit => audit.Answers)
            .ThenInclude(answer => answer.Question)
            .SingleOrDefaultAsync(audit => audit.AuditId.Equals(query.Id), cancellationToken);

        if (audit is null)
        {
            throw new NotFoundException($"Audit with ID {query.Id} does not exist.");
        }

        var auditDto = mapper.Map<Domain.Audit, AuditDto>(audit);
        var result = new GetAuditQueryResult()
        {
            Audit = auditDto
        };

        return result;
    }
}
