using Core.MappingService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Features.Audit.Dto;

public sealed record AuditDto
{
    public required Guid AuditId { get; init; }
    public required string Author { get; init; }
    public required string Area { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required double Score { get; init; }
    public required IReadOnlyList<Answer.Dto.AnswerDto> Answers { get; init; }
    public required IReadOnlyList<AuditAction.Dto.AuditActionDto> Actions { get; init; }
}

public class AuditDtoMapper :
    IMapper<Domain.Audit, AuditDto>,
    IMapper<AuditDto, Api.Contracts.Audit.Dto.AuditDto>
{
    private readonly IMappingService _mapper;

    public AuditDtoMapper(IMappingService mapper)
    {
        _mapper = mapper;
    }

    public AuditDto Map(Domain.Audit src)
    {
        var answers = src.Answers
            .Select(answer => _mapper.Map<Domain.Answer, Answer.Dto.AnswerDto>(answer))
            .ToList();

        var actions = src.Actions
            .Select(action => _mapper.Map<Domain.AuditAction, AuditAction.Dto.AuditActionDto>(action))
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

    public Api.Contracts.Audit.Dto.AuditDto Map(AuditDto src)
    {
        var answers = src.Answers
            .Select(answer => _mapper.Map<Answer.Dto.AnswerDto, Api.Contracts.Answer.Dto.AnswerDto>(answer))
            .ToList();

        var actions = src.Actions
            .Select(action => _mapper.Map<AuditAction.Dto.AuditActionDto, Api.Contracts.AuditAction.Dto.AuditActionDto>(action))
            .ToList();

        var auditDto = new Api.Contracts.Audit.Dto.AuditDto()
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
