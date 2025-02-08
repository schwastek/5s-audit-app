using Api.Requests.Answers.Dto;
using Api.Requests.AuditActions.Dto;
using Core.MappingService;
using Features.Audit.Save;
using System.Linq;

namespace Api.Requests.Audits.Save;

public sealed class SaveAuditRequestMapper : IMapper<SaveAuditRequest, SaveAuditCommand>
{
    private readonly IMappingService _mapper;

    public SaveAuditRequestMapper(IMappingService mapper)
    {
        _mapper = mapper;
    }

    public SaveAuditCommand Map(SaveAuditRequest src)
    {
        var answers = src.Answers
            .Select(_mapper.Map<AnswerForCreationDto, Features.Answer.Dto.AnswerForCreationDto>)
            .ToList();

        var actions = src.Actions
            .Select(_mapper.Map<AuditActionForCreationDto, Features.AuditAction.Dto.AuditActionForCreationDto>)
            .ToList();

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
}

public sealed class SaveAuditCommandResultMapper : IMapper<SaveAuditCommandResult, SaveAuditResponse>
{
    public SaveAuditResponse Map(SaveAuditCommandResult src)
    {
        return new SaveAuditResponse()
        {
            AuditId = src.AuditId
        };
    }
}
