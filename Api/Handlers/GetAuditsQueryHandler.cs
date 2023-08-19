using Api.Core.Domain;
using Api.Data.DbContext;
using Api.Exceptions;
using Api.Helpers;
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

public sealed class GetAuditsQueryHandler : IRequestHandler<GetAuditsQuery, (IEnumerable<AuditListDto> audits, MetaData metaData)>
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

    public async Task<(IEnumerable<AuditListDto> audits, MetaData metaData)> Handle(GetAuditsQuery request, CancellationToken cancellationToken)
    {
        if (!orderByMapping.ValidMappingExists(request.OrderBy))
        {
            throw new IncorrectPropertyBadRequestException(request.OrderBy);
        }

        var sortables = orderByMapping.Map(request.OrderBy);

        var collection = context.Audits
            .Include(audit => audit.Answers)
            .ThenInclude(answer => answer.Question)
            .ApplySort(sortables)
            .AsNoTracking();

        var pagedItems = await PagedList<Audit>.CreateAsync(collection,
            request.PageNumber,
            request.PageSize);

        // Calculate score
        pagedItems.ForEach(audit => audit.CalculateScore());

        // Map
        var auditsDto = mapper.Map<IEnumerable<Audit>, IEnumerable<AuditListDto>>(pagedItems);

        return (audits: auditsDto, metaData: pagedItems.MetaData);
    }
}
