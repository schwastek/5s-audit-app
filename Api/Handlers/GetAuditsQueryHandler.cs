using Api.Core.Domain;
using Api.Data.DbContext;
using Api.Exceptions;
using Api.Extensions;
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

    public GetAuditsQueryHandler(
        LeanAuditorContext context,
        IMappingService mapper,
        OrderByMappingService<AuditListDto, Audit> orderByMapping
    )
    {
        this.context = context;
        this.mapper = mapper;
        this.orderByMapping = orderByMapping;
    }

    public async Task<GetAuditsQueryResult> Handle(GetAuditsQuery request, CancellationToken cancellationToken)
    {
        if (!orderByMapping.ValidMappingExists(request.OrderBy))
        {
            throw new IncorrectPropertyBadRequestException(request.OrderBy);
        }

        var sortables = orderByMapping.Map(request.OrderBy);

        var audits = await context.Audits
            .AsNoTracking()
            .Include(audit => audit.Answers)
            .ThenInclude(answer => answer.Question)
            .ApplySort(sortables)
            .ApplyPaging(request)
            .ToListAsync(cancellationToken);

        // Calculate score
        audits.ForEach(audit => audit.CalculateScore());

        // Map
        var auditsDto = mapper.Map<List<Audit>, List<AuditListDto>>(audits);

        var result = new GetAuditsQueryResult(auditsDto, request);

        return result;
    }
}
