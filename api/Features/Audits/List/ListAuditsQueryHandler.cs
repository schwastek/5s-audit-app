using Data.DbContext;
using Domain;
using Features.Audits.Dto;
using Infrastructure.MediatorService;
using Infrastructure.OrderByService;
using Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Audits.List;

public sealed class ListAuditsQueryHandler : IRequestHandler<ListAuditsQuery, ListAuditsQueryResult>
{
    private readonly LeanAuditorContext _context;
    private readonly OrderByMappingService<ListAuditsQuery, Audit> _orderByMapping;
    private readonly IPaginatedResultFactory<Audit> _paginatedResultFactory;

    public ListAuditsQueryHandler(
        LeanAuditorContext context,
        OrderByMappingService<ListAuditsQuery, Audit> orderByMapping,
        IPaginatedResultFactory<Audit> paginatedResultFactory
    )
    {
        _context = context;
        _orderByMapping = orderByMapping;
        _paginatedResultFactory = paginatedResultFactory;
    }

    public async Task<ListAuditsQueryResult> Handle(ListAuditsQuery query, CancellationToken cancellationToken)
    {
        if (!_orderByMapping.ValidMappingExists(query.OrderBy))
        {
            throw new InvalidOrderByException(query.OrderBy);
        }

        var sortables = _orderByMapping.Map(query.OrderBy);

        var audits = _context.Audits
            .AsNoTracking()
            .Include(audit => audit.Answers)
            .ThenInclude(answer => answer.Question)
            .ApplySort(sortables);

        var paged = await _paginatedResultFactory.CreateAsync(audits, query, cancellationToken);

        // Calculate score
        foreach (var audit in paged.Items)
        {
            audit.CalculateScore();
        }

        // Map
        var auditsDto = paged.Items.Select(x => new AuditListItemDto()
        {
            AuditId = x.AuditId,
            Author = x.Author,
            Area = x.Area,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            Score = x.Score
        }).ToList();

        var result = new ListAuditsQueryResult
        {
            Items = auditsDto,
            Metadata = paged.Metadata
        };

        return result;
    }
}
