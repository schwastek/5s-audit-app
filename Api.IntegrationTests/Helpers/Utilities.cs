using Api.Data;
using Api.Core.Domain;
using System;
using System.Collections.Generic;

namespace Api.IntegrationTests.Helpers;

internal sealed class Utilities
{
    public static void InitializeDbForTests(LeanAuditorContext db)
    {
        // Audits
        // Questions
        // Answers
        // Audit Actions

        var audits = new List<Audit>()
        {
            new Audit
            {
                AuditId = Guid.Parse("f285d889-27b2-4759-9d9f-0fffbdac982b"),
                Author = "TestAuthor1",
                Area = "TestArea1",
                StartDate = DateTime.Parse("2022-11-14T14:02:42.000Z"),
                EndDate = DateTime.Parse("2022-11-14T14:17:30.000Z")
            }
        };

        var questions = new List<Question>()
        {
            new Question
            {
                QuestionId = Guid.Parse("e611c40f-ac06-4d2f-9f95-9c56a98bdf66"),
                QuestionText = "TestQuestion1"
            },
            new Question
            {
                QuestionId = Guid.Parse("699760e6-7f1e-4ac8-98d2-4011b746aa29"),
                QuestionText = "TestQuestion2"
            }
        };

        var answers = new List<Answer>()
        {
            // Answers for audit #1
            new Answer
            {
                AuditId = audits[0].AuditId,
                QuestionId = questions[0].QuestionId,
                AnswerId = Guid.Parse("469c66e1-88b9-4f18-ad5d-32bc4e58aae6"),
                AnswerText = "1",
                AnswerType = "number"
            },
            new Answer
            {
                AuditId = audits[0].AuditId,
                QuestionId = questions[1].QuestionId,
                AnswerId = Guid.Parse("fc966d94-f8f0-426c-8813-32146f1d6ecb"),
                AnswerText = "5",
                AnswerType = "number"
            }
        };

        var auditActions = new List<AuditAction>()
        {
            // Actions for audit #1
            new AuditAction
            {
                AuditId = audits[0].AuditId,
                AuditActionId = Guid.Parse("c1c5b6c9-e862-4adb-a2d1-80687617415b"),
                Description = "TestAuditAction1",
                IsComplete = true
            }
        };

        db.Audits.AddRange(audits);
        db.Questions.AddRange(questions);
        db.Answers.AddRange(answers);
        db.AuditActions.AddRange(auditActions);

        db.SaveChanges();
    }
}
