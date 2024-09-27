using Api.Contracts.Audit.Requests;
using Core.MappingService;
using Core.OrderByService;
using Core.Pagination;
using Features.Audit.Dto;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Features.Audit.List;

public sealed record ListAuditsQuery : IRequest<ListAuditsQueryResult>, IPageableQuery, IOrderByQuery
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required string OrderBy { get; init; }
}

public class ListAuditsQueryResult : IPaginatedResult<AuditListItemDto>
{
    public required IReadOnlyList<AuditListItemDto> Items { get; init; }
    public required PaginationMetadata Metadata { get; init; }
}

public class ListAuditsRequestMapper :
    IMapper<ListAuditsRequest, ListAuditsQuery>,
    IMapper<ListAuditsQueryResult, ListAuditsResponse>
{
    private readonly IMappingService mapper;

    public ListAuditsRequestMapper(IMappingService mapper)
    {
        this.mapper = mapper;
    }

    public ListAuditsQuery Map(ListAuditsRequest src)
    {
        var pageableQuery = new PageableQuery(src.PageNumber, src.PageSize);
        var orderByQuery = new OrderByQuery(src.OrderBy);

        return new ListAuditsQuery()
        {
            OrderBy = orderByQuery.OrderBy,
            PageNumber = pageableQuery.PageNumber,
            PageSize = pageableQuery.PageSize
        };
    }

    public ListAuditsResponse Map(ListAuditsQueryResult src)
    {
        var metadata = mapper.Map<PaginationMetadata, Api.Contracts.Common.Requests.PaginationMetadata>(src.Metadata);

        var auditListItems = src.Items
            .Select(answer => mapper.Map<AuditListItemDto, Api.Contracts.Audit.Dto.AuditListItemDto>(answer))
            .ToList();

        return new ListAuditsResponse()
        {
            Items = auditListItems,
            Metadata = metadata
        };
    }
}
