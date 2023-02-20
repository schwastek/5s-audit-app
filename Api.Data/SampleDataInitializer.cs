using Api.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data;

public class SampleDataInitializer
{
    public static async Task Seed(LeanAuditorContext context)
    {
        if (context.Audits.Any())
        {
            // DB has been seeded
            return;
        }

        // Start clean
        ClearData(context);

        // Audits
        var audits = new List<Audit>()
        {
            new Audit
            {
                AuditId = Guid.Parse("f4940d26-7c0a-4ab6-b1cd-da8f708c5819"),
                Author = "John",
                Area = "warehouse",
                StartDate = DateTime.Parse("2020-01-25T20:06:38.946Z"),
                EndDate = DateTime.Parse("2020-01-25T20:36:38.000Z")
            },
            new Audit
            {
                AuditId = Guid.Parse("a065c86d-3846-41bf-a268-423c743ca064"),
                Author = "John",
                Area = "assembly",
                StartDate = DateTime.Parse("2020-01-25T20:06:38.946Z"),
                EndDate = DateTime.Parse("2020-01-25T20:36:38.000Z")
            }
        };

        // Questions
        var questions = new List<Question>()
        {
            new Question
            {
                QuestionId = Guid.Parse("af70e9f3-9a70-4178-80dc-87d38bb1c810"),
                QuestionText = "Are all tools in the work area currently in use?"
            },
            new Question
            {
                QuestionId = Guid.Parse("cb1a8298-9eca-4684-8c7c-1ed24298cca2"),
                QuestionText = "Are all tools or parts off the floor?"
            },
            new Question
            {
                QuestionId = Guid.Parse("6efa4fdb-b626-4af0-aea8-0a38399502d0"),
                QuestionText = "Are all posted work instructions, notes and drawing currently in use?"
            },
            new Question
            {
                QuestionId = Guid.Parse("bfab9a5c-4e74-4eff-a6ed-7b39748378ad"),
                QuestionText = "Are workstations and walkways clear of unnecessary items and clutter?"
            },
            new Question
            {
                QuestionId = Guid.Parse("eb2ef0b3-6af5-4d53-89d8-7b069ccb343c"),
                QuestionText = "Are occasionally used items stored separately?"
            }
        };

        // Answers
        var answers = new List<Answer>()
        {
            // Answers for audit #1
            new Answer
            {
                AuditId = audits[0].AuditId,
                QuestionId = questions[0].QuestionId,
                AnswerId = Guid.Parse("76551e14-0791-4d3b-99fd-387e25b9c96b"),
                AnswerText = "1",
                AnswerType = "number"
            },
            new Answer
            {
                AuditId = audits[0].AuditId,
                QuestionId = questions[1].QuestionId,
                AnswerId = Guid.Parse("0c24c0d3-2092-4266-ba7c-9e36c7b3256d"),
                AnswerText = "2",
                AnswerType = "number"
            },
            new Answer
            {
                AuditId = audits[0].AuditId,
                QuestionId = questions[2].QuestionId,
                AnswerId = Guid.Parse("dd3ae4e3-9651-4142-9a4d-63e33cad11cb"),
                AnswerText = "3",
                AnswerType = "number"
            },
            new Answer
            {
                AuditId = audits[0].AuditId,
                QuestionId = questions[3].QuestionId,
                AnswerId = Guid.Parse("30deb494-6e45-4eb5-ab99-cc2f8eac981b"),
                AnswerText = "4",
                AnswerType = "number"
            },
            new Answer
            {
                AuditId = audits[0].AuditId,
                QuestionId = questions[4].QuestionId,
                AnswerId = Guid.Parse("690da001-2594-4f64-a7d5-68b1fa095493"),
                AnswerText = "5",
                AnswerType = "number"
            },
            // Answers for audit #2
            new Answer
            {
                AuditId = audits[1].AuditId,
                QuestionId = questions[0].QuestionId,
                AnswerId = Guid.Parse("fcaf6a07-c661-4977-bc81-ce4f0675b344"),
                AnswerText = "1",
                AnswerType = "number"
            },
            new Answer
            {
                AuditId = audits[1].AuditId,
                QuestionId = questions[1].QuestionId,
                AnswerId = Guid.Parse("1939baab-b109-42b6-be49-7eee38575f69"),
                AnswerText = "1",
                AnswerType = "number"
            },
            new Answer
            {
                AuditId = audits[1].AuditId,
                QuestionId = questions[2].QuestionId,
                AnswerId = Guid.Parse("833bd0a9-2e2c-4a4d-ab7e-59597324f191"),
                AnswerText = "1",
                AnswerType = "number"
            },
            new Answer
            {
                AuditId = audits[1].AuditId,
                QuestionId = questions[3].QuestionId,
                AnswerId = Guid.Parse("4d539fc1-bad0-4380-90a4-1ed9d305150b"),
                AnswerText = "1",
                AnswerType = "number"
            },
            new Answer
            {
                AuditId = audits[1].AuditId,
                QuestionId = questions[4].QuestionId,
                AnswerId = Guid.Parse("cc0e98ee-090d-4046-af4c-7bd1e05994b9"),
                AnswerText = "1",
                AnswerType = "number"
            }
        };

        // Actions
        var auditActions = new List<AuditAction>()
        {
            // Actions for audit #1
            new AuditAction
            {
                AuditId = audits[0].AuditId,
                AuditActionId = Guid.Parse("33a4a3d5-54dc-4bcb-a27f-0d469f6adca4"),
                Description = "Clean up the workplace",
                IsComplete = false
            },
            // Actions for audit #2
            new AuditAction
            {
                AuditId = audits[1].AuditId,
                AuditActionId = Guid.Parse("330019f1-797e-4933-b9ed-2082256d45d8"),
                Description = "Remove obsolete materials",
                IsComplete = false
            }
        };

        context.Audits.AddRange(audits);
        context.Questions.AddRange(questions);
        context.Answers.AddRange(answers);
        context.AuditActions.AddRange(auditActions);

        await context.SaveChangesAsync();
    }

    private static void ClearData(LeanAuditorContext context)
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
            var entity = context.Model.FindEntityType(entityName);
            var tableName = entity.GetTableName();
            context.Database.ExecuteSqlRaw($"DELETE FROM {tableName}");
        }
    }
}
