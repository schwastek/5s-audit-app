using Api.Common;
using Api.Core.Domain;
using Api.Data.DbContext;
using Api.Exceptions;
using Api.Mappers;
using Api.Mappers.OrderBy;
using Api.Models;
using Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Handlers;

public sealed class GetAuditsQueryHandler : IRequestHandler<GetAuditsQuery, GetAuditsQueryResult>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;
    private readonly OrderByMappingService<AuditListDto, Audit> orderByMapping;
    private readonly IPaginatedResultFactory<Audit> paginatedResultFactory;

    public GetAuditsQueryHandler(
        LeanAuditorContext context,
        IMappingService mapper,
        OrderByMappingService<AuditListDto, Audit> orderByMapping,
        IPaginatedResultFactory<Audit> paginatedResultFactory
    )
    {
        this.context = context;
        this.mapper = mapper;
        this.orderByMapping = orderByMapping;
        this.paginatedResultFactory = paginatedResultFactory;
    }

    public async Task<GetAuditsQueryResult> Handle(GetAuditsQuery request, CancellationToken cancellationToken)
    {
        if (!orderByMapping.ValidMappingExists(request.OrderBy))
        {
            throw new IncorrectPropertyBadRequestException(request.OrderBy);
        }

        var sortables = orderByMapping.Map(request.OrderBy);

        var audits = context.Audits
            .AsNoTracking()
            .Include(audit => audit.Answers)
            .ThenInclude(answer => answer.Question)
            .ApplySort(sortables);

        var paged = await paginatedResultFactory.CreateAsync(audits, request, cancellationToken);

        // Calculate score
        foreach (var audit in paged.Items)
        {
            audit.CalculateScore();
        }

        // Map
        var auditsDto = mapper.Map<IReadOnlyList<Audit>, IReadOnlyList<AuditListDto>>(paged.Items);

        var result = new GetAuditsQueryResult
        {
            Items = auditsDto,
            Metadata = paged.Metadata
        };

        return result;
    }
}
