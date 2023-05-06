using Api.Core.Domain;
using Api.Exceptions;
using Api.Helpers;
using Api.Mappers;
using Api.Models;
using Api.Queries;
using Api.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Data.DbContext;

namespace Api.Handlers;

public sealed class GetAuditsHandler : IRequestHandler<GetAuditsQuery, (IEnumerable<AuditListDto> audits, MetaData metaData)>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;
    private readonly IPropertyMappingService propertyMappingService;

    public GetAuditsHandler(LeanAuditorContext context, IMappingService mapper,
        IPropertyMappingService propertyMappingService)
    {
        this.context = context;
        this.mapper = mapper;
        this.propertyMappingService = propertyMappingService;
    }

    public async Task<(IEnumerable<AuditListDto> audits, MetaData metaData)> Handle(GetAuditsQuery request, CancellationToken cancellationToken)
    {
        if (!propertyMappingService.ValidMappingExistsFor<AuditDto, Audit>
            (request.QueryParameters.OrderBy))
        {
            throw new IncorrectPropertyBadRequestException(request.QueryParameters.OrderBy);
        }

        IQueryable<Audit> collection = context.Audits
            .Include(audit => audit.Answers)
            .ThenInclude(answer => answer.Question)
            .AsNoTracking()
            .AsQueryable();

        // Apply sorting
        bool hasOrderBy = !string.IsNullOrWhiteSpace(request.QueryParameters.OrderBy);

        if (hasOrderBy)
        {
            Dictionary<string, PropertyMappingValue> auditPropertyMapping =
                propertyMappingService.GetPropertyMapping<AuditDto, Audit>();

            collection = collection.ApplySort(request.QueryParameters.OrderBy,
                auditPropertyMapping);
        }

        PagedList<Audit> pagedItems = await PagedList<Audit>.CreateAsync(collection,
            request.QueryParameters.PageNumber,
            request.QueryParameters.PageSize);

        // Calculate score
        pagedItems.ForEach(audit => audit.CalculateScore());

        // Map
        var auditsDto = mapper.Map<IEnumerable<Audit>, IEnumerable<AuditListDto>>(pagedItems);

        return (audits: auditsDto, metaData: pagedItems.MetaData);
    }
}
