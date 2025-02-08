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
            .FirstAsync(audit => audit.AuditId.Equals(query.Id), cancellationToken);

        var auditDto = mapper.Map<Domain.Audit, AuditDto>(audit);
        var result = new GetAuditQueryResult()
        {
            AuditId = auditDto.AuditId,
            Author = auditDto.Author,
            Area = auditDto.Area,
            StartDate = auditDto.StartDate,
            EndDate = auditDto.EndDate,
            Actions = auditDto.Actions,
            Answers = auditDto.Answers,
            Score = auditDto.Score
        };

        return result;
    }
}
