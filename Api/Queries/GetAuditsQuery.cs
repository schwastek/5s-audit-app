using Api.Common;
using Api.Models;
using MediatR;

namespace Api.Queries;

public sealed record GetAuditsQuery :
    IRequest<GetAuditsQueryResult>,
    IPageableQuery,
    IOrderByQuery
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string OrderBy { get; init; } = string.Empty;
}

public class GetAuditsQueryResult : PaginatedResult<AuditListDto>
{

}
