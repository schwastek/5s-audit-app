using Core.MappingService;
using Core.OrderByService;
using Core.Pagination;
using Data.DbContext;
using Domain;
using Features.Audits.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Audits.List;

public sealed class ListAuditsQueryHandler : IRequestHandler<ListAuditsQuery, ListAuditsQueryResult>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;
    private readonly OrderByMappingService<ListAuditsQuery, Audit> orderByMapping;
    private readonly IPaginatedResultFactory<Audit> paginatedResultFactory;

    public ListAuditsQueryHandler(
        LeanAuditorContext context,
        IMappingService mapper,
        OrderByMappingService<ListAuditsQuery, Audit> orderByMapping,
        IPaginatedResultFactory<Audit> paginatedResultFactory
    )
    {
        this.context = context;
        this.mapper = mapper;
        this.orderByMapping = orderByMapping;
        this.paginatedResultFactory = paginatedResultFactory;
    }

    public async Task<ListAuditsQueryResult> Handle(ListAuditsQuery query, CancellationToken cancellationToken)
    {
        if (!orderByMapping.ValidMappingExists(query.OrderBy))
        {
            throw new InvalidOrderByException(query.OrderBy);
        }

        var sortables = orderByMapping.Map(query.OrderBy);

        var audits = context.Audits
            .AsNoTracking()
            .Include(audit => audit.Answers)
            .ThenInclude(answer => answer.Question)
            .ApplySort(sortables);

        var paged = await paginatedResultFactory.CreateAsync(audits, query, cancellationToken);

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
