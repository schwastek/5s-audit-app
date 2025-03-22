using Data.DbContext;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data.Seed;

public class SampleDataSeeder
{
    public static async Task SeedAsync(LeanAuditorContext context)
    {
        // Audits
        var audits = new List<Audit>()
        {
            Audit.Create(
                auditId: Guid.Parse("f4940d26-7c0a-4ab6-b1cd-da8f708c5819"),
                author: "John",
                area: "warehouse",
                startDate: DateTime.Parse("2020-01-25T20:06:38.946Z"),
                endDate: DateTime.Parse("2020-01-25T20:36:38.000Z")
            ),
            Audit.Create(
                auditId: Guid.Parse("a065c86d-3846-41bf-a268-423c743ca064"),
                author: "John",
                area: "assembly",
                startDate: DateTime.Parse("2020-01-25T20:06:38.946Z"),
                endDate: DateTime.Parse("2020-01-25T20:36:38.000Z")
            )
        };

        // Audit Actions
        audits[0].AddActions(
            AuditAction.Create(
                auditActionId: Guid.Parse("33a4a3d5-54dc-4bcb-a27f-0d469f6adca4"),
                description: "Clean up the workplace"
            )
        );

        audits[1].AddActions(
            AuditAction.Create(
                auditActionId: Guid.Parse("330019f1-797e-4933-b9ed-2082256d45d8"),
                description: "Remove obsolete materials"
            )
        );

        // Questions
        var questions = new List<Question>()
        {
            Question.Create(
                questionId: Guid.Parse("af70e9f3-9a70-4178-80dc-87d38bb1c810"),
                questionText: "Are all tools in the work area currently in use?"
            ),
            Question.Create(
                questionId: Guid.Parse("cb1a8298-9eca-4684-8c7c-1ed24298cca2"),
                questionText: "Are all tools or parts off the floor?"
            ),
            Question.Create(
                questionId: Guid.Parse("6efa4fdb-b626-4af0-aea8-0a38399502d0"),
                questionText: "Are all posted work instructions, notes and drawing currently in use?"
            ),
            Question.Create(
                questionId: Guid.Parse("bfab9a5c-4e74-4eff-a6ed-7b39748378ad"),
                questionText: "Are workstations and walkways clear of unnecessary items and clutter?"
            ),
            Question.Create(
                questionId: Guid.Parse("eb2ef0b3-6af5-4d53-89d8-7b069ccb343c"),
                questionText: "Are occasionally used items stored separately?"
            )
        };

        // Answers
        audits[0].AddAnswers([
            Answer.Create(
                answerId: Guid.Parse("76551e14-0791-4d3b-99fd-387e25b9c96b"),
                questionId: questions[0].QuestionId,
                answerText: "1",
                answerType: "number"
            ),
            Answer.Create(
                answerId: Guid.Parse("0c24c0d3-2092-4266-ba7c-9e36c7b3256d"),
                questionId: questions[1].QuestionId,
                answerText: "2",
                answerType: "number"
            ),
            Answer.Create(
                answerId: Guid.Parse("dd3ae4e3-9651-4142-9a4d-63e33cad11cb"),
                questionId: questions[2].QuestionId,
                answerText: "3",
                answerType: "number"
            ),
            Answer.Create(
                answerId: Guid.Parse("30deb494-6e45-4eb5-ab99-cc2f8eac981b"),
                questionId: questions[3].QuestionId,
                answerText: "4",
                answerType: "number"
            ),
            Answer.Create(
                answerId: Guid.Parse("690da001-2594-4f64-a7d5-68b1fa095493"),
                questionId: questions[4].QuestionId,
                answerText: "5",
                answerType: "number"
            )
        ]);

        audits[1].AddAnswers([
            Answer.Create(
                answerId: Guid.Parse("fcaf6a07-c661-4977-bc81-ce4f0675b344"),
                questionId: questions[0].QuestionId,
                answerText: "1",
                answerType: "number"
            ),
            Answer.Create(
                answerId: Guid.Parse("1939baab-b109-42b6-be49-7eee38575f69"),
                questionId: questions[1].QuestionId,
                answerText: "1",
                answerType: "number"
            ),
            Answer.Create(
                answerId: Guid.Parse("833bd0a9-2e2c-4a4d-ab7e-59597324f191"),
                questionId: questions[2].QuestionId,
                answerText: "1",
                answerType: "number"
            ),
            Answer.Create(
                answerId: Guid.Parse("4d539fc1-bad0-4380-90a4-1ed9d305150b"),
                questionId: questions[3].QuestionId,
                answerText: "1",
                answerType: "number"
            ),
            Answer.Create(
                answerId: Guid.Parse("cc0e98ee-090d-4046-af4c-7bd1e05994b9"),
                questionId: questions[4].QuestionId,
                answerText: "1",
                answerType: "number"
            )
        ]);

        context.Audits.AddRange(audits);
        context.Questions.AddRange(questions);

        await context.SaveChangesAsync();
    }

    public async static Task ClearAsync(LeanAuditorContext context)
    {
        var entities = new[]
        {
            typeof(Audit).FullName,
            typeof(Question).FullName,
            typeof(Answer).FullName,
            typeof(AuditAction).FullName,
        };

        foreach (var entityName in entities)
        {
            var entity = context.Model.FindEntityType(entityName!);
            var tableName = entity!.GetTableName();
#pragma warning disable EF1002 // Risk of vulnerability to SQL injection.
            context.Database.ExecuteSqlRaw($"DELETE FROM {tableName}");
#pragma warning restore EF1002 // Risk of vulnerability to SQL injection.
        }

        await context.SaveChangesAsync();
    }
}
