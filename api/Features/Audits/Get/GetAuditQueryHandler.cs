using Data.DbContext;
using Domain;
using Features.Audits.Dto;
using Features.Core.MappingService;
using Features.Core.MediatorService;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Audits.Get;

public sealed class GetAuditQueryHandler : IRequestHandler<GetAuditQuery, GetAuditQueryResult>
{
    private readonly LeanAuditorContext _context;
    private readonly IMappingService _mapper;

    public GetAuditQueryHandler(LeanAuditorContext context, IMappingService mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetAuditQueryResult> Handle(GetAuditQuery query, CancellationToken cancellationToken)
    {
        var audit = await _context.Audits
            .AsNoTracking()
            .Include(audit => audit.Actions)
            .Include(audit => audit.Answers)
            .ThenInclude(answer => answer.Question)
            .FirstAsync(audit => audit.AuditId.Equals(query.Id), cancellationToken);

        var auditDto = _mapper.Map<Audit, AuditDto>(audit);
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
