using Core.MappingService;
using System.Linq;

namespace Features.Audit.Dto;

public sealed class AuditDtoMapper : IMapper<Domain.Audit, AuditDto>
{
    private readonly IMappingService _mapper;

    public AuditDtoMapper(IMappingService mapper)
    {
        _mapper = mapper;
    }

    public AuditDto Map(Domain.Audit src)
    {
        var answers = src.Answers
            .Select(_mapper.Map<Domain.Answer, Answer.Dto.AnswerDto>)
            .ToList();

        var actions = src.Actions
            .Select(_mapper.Map<Domain.AuditAction, AuditAction.Dto.AuditActionDto>)
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
