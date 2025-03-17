using Features.Answers.Dto;
using Features.AuditActions.Dto;
using Features.Audits.Save;
using Features.Core.MappingService;
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
            .Select(_mapper.Map<Requests.Answers.Dto.AnswerForCreationDto, AnswerForCreationDto>)
            .ToList();

        var actions = src.Actions
            .Select(_mapper.Map<Requests.AuditActions.Dto.AuditActionForCreationDto, AuditActionForCreationDto>)
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
