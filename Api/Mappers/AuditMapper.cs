using Api.Core.Domain;
using Api.Models;
using System.Linq;

namespace Api.Mappers;

public class AuditMapper : 
    IMapper<AuditForCreationDto, Audit>, IMapper<Audit, AuditDto>, IMapper<Audit, AuditListDto>
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
            Score = audit.CalculateScore(),
            Answers = answers,
            Actions = actions
        };

        return auditDto;
    }

    AuditListDto IMapper<Audit, AuditListDto>.Map(Audit audit)
    {
        var auditDto = new AuditListDto()
        {
            AuditId = audit.AuditId,
            Area = audit.Area,
            Author = audit.Author,
            StartDate = audit.StartDate,
            EndDate = audit.EndDate,
            Score = audit.CalculateScore()
        };

        return auditDto;
    }
}
