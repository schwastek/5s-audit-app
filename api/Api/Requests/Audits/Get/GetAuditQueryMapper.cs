using Features.Answers.Dto;
using Features.AuditActions.Dto;
using Features.Audits.Get;
using Features.Core.MappingService;
using System.Linq;

namespace Api.Requests.Audits.Get;

public sealed class GetAuditRequestMapper : IMapper<GetAuditRequest, GetAuditQuery>
{
    public GetAuditQuery Map(GetAuditRequest src)
    {
        return new GetAuditQuery()
        {
            Id = src.Id
        };
    }
}

public sealed class GetAuditQueryResultMapper : IMapper<GetAuditQueryResult, GetAuditResponse>
{
    private readonly IMappingService _mapper;

    public GetAuditQueryResultMapper(IMappingService mapper)
    {
        _mapper = mapper;
    }

    public GetAuditResponse Map(GetAuditQueryResult src)
    {
        var answers = src.Answers
                .Select(_mapper.Map<AnswerDto, Requests.Answers.Dto.AnswerDto>)
                .ToList();

        var actions = src.Actions
            .Select(_mapper.Map<AuditActionDto, Requests.AuditActions.Dto.AuditActionDto>)
            .ToList();

        var response = new GetAuditResponse()
        {
            AuditId = src.AuditId,
            Author = src.Author,
            Area = src.Area,
            StartDate = src.StartDate,
            EndDate = src.EndDate,
            Score = src.Score,
            Answers = answers,
            Actions = actions
        };

        return response;
    }
}
