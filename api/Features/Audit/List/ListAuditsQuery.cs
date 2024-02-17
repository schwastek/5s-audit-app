using Api.Contracts.Audit.Requests;
using Api.Mappers.MappingService;
using Core.MappingService;
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
    public IPaginationMetadata Metadata { get; init; } = null!;
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
        const string defaultOrderBy = "author asc";

        return new ListAuditsQuery()
        {
            OrderBy = src.OrderBy ?? defaultOrderBy,
            PageNumber = PageableRequest.GetPageNumber(src.PageNumber),
            PageSize = PageableRequest.GetPageSize(src.PageSize)
        };
    }

    public ListAuditsResponse Map(ListAuditsQueryResult src)
    {
        var metadata = mapper.Map<IPaginationMetadata, Api.Contracts.Common.Requests.IPaginationMetadata>(src.Metadata);

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
