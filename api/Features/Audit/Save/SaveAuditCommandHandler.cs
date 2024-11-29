using Data.DbContext;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Audit.Save;

public sealed class SaveAuditCommandHandler : IRequestHandler<SaveAuditCommand, SaveAuditCommandResult>
{
    private readonly LeanAuditorContext context;

    public SaveAuditCommandHandler(LeanAuditorContext context)
    {
        this.context = context;
    }

    public async Task<SaveAuditCommandResult> Handle(SaveAuditCommand command, CancellationToken cancellationToken)
    {
        // Create entities
        var answers = command.Answers.Select(answer => Domain.Answer.Create(
            answerId: answer.AnswerId,
            questionId: answer.QuestionId,
            answerText: answer.AnswerText,
            answerType: answer.AnswerType
        ));

        var actions = command.Actions.Select(action => Domain.AuditAction.Create(
            auditActionId: action.ActionId,
            description: action.Description
        ));

        var audit = Domain.Audit.Create(
            auditId: command.AuditId,
            author: command.Author,
            area: command.Area,
            startDate: command.StartDate,
            endDate: command.EndDate
        );

        // Save
        audit.AddAnswers(answers);
        audit.AddActions(actions);

        context.Audits.Add(audit);
        context.SaveChanges();

        audit.CalculateScore();

        return await Task.FromResult(new SaveAuditCommandResult() { AuditId = audit.AuditId });
    }
}
