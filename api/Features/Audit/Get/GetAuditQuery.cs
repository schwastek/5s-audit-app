using Api.Contracts.Audit.Requests;
using Core.MappingService;
using MediatR;
using System;

namespace Features.Audit.Get;

public sealed record GetAuditQuery : IRequest<GetAuditQueryResult>
{
    public Guid Id { get; init; }
}

public sealed record GetAuditQueryResult
{
    public Dto.AuditDto Audit { get; init; } = null!;
}

public class GetAuditQueryMapper :
    IMapper<GetAuditRequest, GetAuditQuery>,
    IMapper<GetAuditQueryResult, GetAuditResponse>
{
    private readonly IMappingService mapper;

    public GetAuditQueryMapper(IMappingService mapper)
    {
        this.mapper = mapper;
    }

    public GetAuditQuery Map(GetAuditRequest source)
    {
        var result = new GetAuditQuery()
        {
            Id = source.Id
        };

        return result;
    }

    public GetAuditResponse Map(GetAuditQueryResult src)
    {
        var auditDto = mapper.Map<Dto.AuditDto, Api.Contracts.Audit.Dto.AuditDto>(src.Audit);

        return new GetAuditResponse()
        {
            Audit = auditDto
        };
    }
}
