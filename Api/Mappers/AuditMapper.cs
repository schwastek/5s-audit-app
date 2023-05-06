using Api.Core.Domain;
using Api.Models;
using Api.Queries;
using Api.Requests;
using System.Collections.Generic;
using System.Linq;

namespace Api.Mappers;

public class AuditMapper : 
    IMapper<AuditForCreationDto, Audit>, IMapper<Audit, AuditDto>, IMapper<IEnumerable<Audit>, IEnumerable<AuditListDto>>,
    IMapper<GetAuditsRequest, GetAuditsQuery>
{
    private readonly IMappingService _mapper;

    public AuditMapper(IMappingService mapper)
    {
        _mapper = mapper;
    }

    public Audit Map(AuditForCreationDto request)
    {
        var answers = request.Answers
            .Select(answer => _mapper.Map<AnswerForCreationDto, Answer>(answer))
            .ToList();

        var actions = request.Actions
            .Select(a => _mapper.Map<AuditActionForCreationDto, AuditAction>(a))
            .ToList();

        Audit audit = new()
        {
            AuditId = request.AuditId,
            Author = request.Author,
            Area = request.Area,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Answers = answers,
            Actions = actions
        };

        return audit;
    }

    public AuditDto Map(Audit audit)
    {
        var answers = audit.Answers
            .Select(answer => _mapper.Map<Answer, AnswerDto>(answer))
            .ToList();

        var actions = audit.Actions
            .Select(a => _mapper.Map<AuditAction, AuditActionDto>(a))
            .ToList();

        AuditDto auditDto = new()
        {
            AuditId = audit.AuditId,
            Area = audit.Area,
            Author = audit.Author,
            StartDate = audit.StartDate,
            EndDate = audit.EndDate,
            Score = audit.Score,
            Answers = answers,
            Actions = actions
        };

        return auditDto;
    }

    public IEnumerable<AuditListDto> Map(IEnumerable<Audit> audits)
    {
        var result = audits.Select(audit => new AuditListDto()
        {
            AuditId = audit.AuditId,
            Area = audit.Area,
            Author = audit.Author,
            StartDate = audit.StartDate,
            EndDate = audit.EndDate,
            Score = audit.Score
        });

        return result;
    }

    public GetAuditsQuery Map(GetAuditsRequest entity)
    {
        var result = new GetAuditsQuery()
        {
            PageNumber = entity.PageNumber,
            PageSize = entity.PageSize,
            OrderBy = entity.OrderBy
        };

        return result;
    }
}
