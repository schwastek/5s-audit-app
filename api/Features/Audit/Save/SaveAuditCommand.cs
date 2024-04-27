using Api.Contracts.Audit.Requests;
using Api.Mappers.MappingService;
using Core.MappingService;
using Features.Audit.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Features.Audit.Save;

public sealed record SaveAuditCommand : IRequest<SaveAuditCommandResult>
{
    public Guid AuditId { get; init; }
    public string Author { get; init; } = null!;
    public string Area { get; init; } = null!;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public ICollection<Answer.Dto.AnswerForCreationDto> Answers { get; init; } = null!;
    public ICollection<AuditAction.Dto.AuditActionForCreationDto> Actions { get; init; } = null!;
}

public sealed record SaveAuditCommandResult
{
    public AuditDto Audit { get; init; } = null!;
}

public class SaveAuditCommandMapper :
    IMapper<SaveAuditRequest, SaveAuditCommand>,
    IMapper<SaveAuditCommandResult, SaveAuditResponse>
{
    private readonly IMappingService _mapper;

    public SaveAuditCommandMapper(IMappingService mapper)
    {
        _mapper = mapper;
    }

    public SaveAuditCommand Map(SaveAuditRequest src)
    {
        var answers = src.Answers?
            .Select(answer => _mapper.Map<Api.Contracts.Answer.Dto.AnswerForCreationDto, Answer.Dto.AnswerForCreationDto>(answer))
            .ToList() ?? new List<Answer.Dto.AnswerForCreationDto>();

        var actions = src.Actions?
            .Select(action => _mapper.Map<Api.Contracts.AuditAction.Dto.AuditActionForCreationDto, AuditAction.Dto.AuditActionForCreationDto>(action))
            .ToList() ?? new List<AuditAction.Dto.AuditActionForCreationDto>();

        var command = new SaveAuditCommand()
        {
            AuditId = src.AuditId,
            Author = src.Author,
            Area = src.Area ?? string.Empty,
            StartDate = src.StartDate,
            EndDate = src.EndDate,
            Answers = answers,
            Actions = actions
        };

        return command;
    }

    public SaveAuditResponse Map(SaveAuditCommandResult src)
    {
        var auditDto = _mapper.Map<AuditDto, Api.Contracts.Audit.Dto.AuditDto>(src.Audit);

        return new SaveAuditResponse()
        {
            Audit = auditDto
        };
    }
}
