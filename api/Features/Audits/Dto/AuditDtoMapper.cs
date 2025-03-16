using Core.MappingService;
using Domain;
using Features.Answers.Dto;
using Features.AuditActions.Dto;
using System.Linq;

namespace Features.Audits.Dto;

public sealed class AuditDtoMapper : IMapper<Audit, AuditDto>
{
    private readonly IMappingService _mapper;

    public AuditDtoMapper(IMappingService mapper)
    {
        _mapper = mapper;
    }

    public AuditDto Map(Audit src)
    {
        var answers = src.Answers
            .Select(_mapper.Map<Answer, AnswerDto>)
            .ToList();

        var actions = src.Actions
            .Select(_mapper.Map<AuditAction, AuditActionDto>)
            .ToList();

        var auditDto = new AuditDto()
        {
            AuditId = src.AuditId,
            Area = src.Area,
            Author = src.Author,
            StartDate = src.StartDate,
            EndDate = src.EndDate,
            Score = src.Score,
            Answers = answers,
            Actions = actions
        };

        return auditDto;
    }
}
