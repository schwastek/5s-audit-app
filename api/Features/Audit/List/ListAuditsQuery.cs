using Api.Contracts.Audit.Requests;
using Core.MappingService;
using Core.OrderByService;
using Core.Pagination;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Features.Audit.List;

public sealed record ListAuditsQuery : IRequest<ListAuditsQueryResult>, IPageableQuery, IOrderByQuery
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string OrderBy { get; init; } = null!;
}

public class ListAuditsQueryResult : IPaginatedResult<Dto.AuditListItemDto>
{
    // TODO: Change `null!` to `required` when C# 11
    public IReadOnlyList<Dto.AuditListItemDto> Items { get; init; } = null!;
    public PaginationMetadata Metadata { get; init; } = null!;
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
            .Select(answer => mapper.Map<Dto.AuditListItemDto, Api.Contracts.Audit.Dto.AuditListItemDto>(answer))
            .ToList();

        return new ListAuditsResponse()
        {
            Items = auditListItems,
            Metadata = metadata
        };
    }
}
