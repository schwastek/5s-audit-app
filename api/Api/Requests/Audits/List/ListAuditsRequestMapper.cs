using Features.Audits.Dto;
using Features.Audits.List;
using Features.Core.MappingService;
using Features.Core.OrderByService;
using Features.Core.Pagination;
using System.Linq;

namespace Api.Requests.Audits.List;

public sealed class ListAuditsRequestMapper : IMapper<ListAuditsRequest, ListAuditsQuery>
{
    public ListAuditsQuery Map(ListAuditsRequest src)
    {
        var orderByQuery = new OrderByQuery(src.OrderBy);
        var pageableQuery = new PageableQuery(src.PageNumber, src.PageSize);

        return new ListAuditsQuery
        {
            OrderBy = orderByQuery.OrderBy,
            PageNumber = pageableQuery.PageNumber,
            PageSize = pageableQuery.PageSize
        };
    }
}

public sealed class ListAuditsQueryResultMapper : IMapper<ListAuditsQueryResult, ListAuditsResponse>
{
    private readonly IMappingService _mapper;

    public ListAuditsQueryResultMapper(IMappingService mapper)
    {
        _mapper = mapper;
    }

    public ListAuditsResponse Map(ListAuditsQueryResult src)
    {
        var items = src.Items
            .Select(_mapper.Map<AuditListItemDto, Requests.Audits.Dto.AuditListItemDto>)
            .ToList();

        var metadata = _mapper.Map<Features.Core.Pagination.PaginationMetadata, Requests.Common.PaginationMetadata>(src.Metadata);

        return new ListAuditsResponse()
        {
            Items = items,
            Metadata = metadata
        };
    }
}
