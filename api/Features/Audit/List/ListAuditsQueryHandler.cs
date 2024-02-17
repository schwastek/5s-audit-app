using Core.OrderByService;
using Core.Pagination;
using Data.DbContext;
using Features.Audit.Dto;
using Api.Mappers.MappingService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Audit.List;

public sealed class ListAuditsQueryHandler : IRequestHandler<ListAuditsQuery, ListAuditsQueryResult>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;
    private readonly OrderByMappingService<ListAuditsQuery, Domain.Audit> orderByMapping;
    private readonly IPaginatedResultFactory<Domain.Audit> paginatedResultFactory;

    public ListAuditsQueryHandler(
        LeanAuditorContext context,
        IMappingService mapper,
        OrderByMappingService<ListAuditsQuery, Domain.Audit> orderByMapping,
        IPaginatedResultFactory<Domain.Audit> paginatedResultFactory
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
            throw new IncorrectPropertyBadRequestException(query.OrderBy);
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
        var auditsDto = paged.Items
            .Select(audit => mapper.Map<Domain.Audit, AuditListItemDto>(audit))
            .ToList();

        var result = new ListAuditsQueryResult
        {
            Items = auditsDto,
            Metadata = paged.Metadata
        };

        return result;
    }
}
