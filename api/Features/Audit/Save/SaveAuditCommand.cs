using Api.Contracts.Audit.Requests;
using Core.MappingService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Features.Audit.Save;

public sealed record SaveAuditCommand : IRequest<SaveAuditCommandResult>
{
    public required Guid AuditId { get; init; }
    public required string Author { get; init; }
    public required string Area { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required ICollection<Answer.Dto.AnswerForCreationDto> Answers { get; init; }
    public required ICollection<AuditAction.Dto.AuditActionForCreationDto> Actions { get; init; }
}

public sealed record SaveAuditCommandResult
{
    public Guid AuditId { get; init; }
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
            Area = src.Area,
            StartDate = src.StartDate,
            EndDate = src.EndDate,
            Answers = answers,
            Actions = actions
        };

        return command;
    }

    public SaveAuditResponse Map(SaveAuditCommandResult src)
    {
        return new SaveAuditResponse()
        {
            AuditId = src.AuditId
        };
    }
}
