using Api.Helpers;
using Api.Models;
using MediatR;
using System.Collections.Generic;

namespace Api.Queries;

public sealed record GetAuditsQuery() : IRequest<(IEnumerable<AuditListDto> audits, MetaData metaData)>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string OrderBy { get; init; } = string.Empty;
}