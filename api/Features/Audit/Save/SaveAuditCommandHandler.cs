using Api.Mappers.MappingService;
using Data.DbContext;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Audit.Save;

public sealed class SaveAuditCommandHandler : IRequestHandler<SaveAuditCommand, SaveAuditCommandResult>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;

    public SaveAuditCommandHandler(LeanAuditorContext context, IMappingService mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<SaveAuditCommandResult> Handle(SaveAuditCommand command, CancellationToken cancellationToken)
    {
        // TODO: Use factory.
        var answers = command.Answers.Select(answer => new Domain.Answer()
        {
            AuditId = command.AuditId,
            AnswerId = answer.AnswerId,
            AnswerText = answer.AnswerText,
            AnswerType = answer.AnswerType,
            QuestionId = answer.QuestionId
        }).ToList();

        var actions = command.Actions.Select(action => new Domain.AuditAction()
        {
            AuditId = command.AuditId,
            AuditActionId = action.ActionId,
            Description = action.Description,
            IsComplete = action.IsComplete
        }).ToList();

        var audit = new Domain.Audit()
        {
            AuditId = command.AuditId,
            Author = command.Author,
            Area = command.Area,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            Answers = answers,
            Actions = actions
        };

        context.Audits.Add(audit);
        context.SaveChanges();

        audit.CalculateScore();

        return await Task.FromResult(new SaveAuditCommandResult() { AuditId = audit.AuditId });
    }
}
