using Data.DbContext;
using Domain;
using Features.Core.MediatorService;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Audits.Save;

public sealed class SaveAuditCommandHandler : IRequestHandler<SaveAuditCommand, SaveAuditCommandResult>
{
    private readonly LeanAuditorContext _context;

    public SaveAuditCommandHandler(LeanAuditorContext context)
    {
        _context = context;
    }

    public async Task<SaveAuditCommandResult> Handle(SaveAuditCommand command, CancellationToken cancellationToken)
    {
        // Create entities
        var answers = command.Answers.Select(answer => Answer.Create(
            answerId: answer.AnswerId,
            questionId: answer.QuestionId,
            answerText: answer.AnswerText,
            answerType: answer.AnswerType
        ));

        var actions = command.Actions.Select(action => AuditAction.Create(
            auditActionId: action.AuditActionId,
            description: action.Description
        ));

        var audit = Audit.Create(
            auditId: command.AuditId,
            author: command.Author,
            area: command.Area,
            startDate: command.StartDate,
            endDate: command.EndDate
        );

        // Save
        audit.AddAnswers(answers);
        audit.AddActions(actions);

        _context.Audits.Add(audit);
        _context.SaveChanges();

        audit.CalculateScore();

        return await Task.FromResult(new SaveAuditCommandResult() { AuditId = audit.AuditId });
    }
}
