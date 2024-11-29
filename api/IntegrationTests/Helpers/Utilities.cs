using Data.DbContext;
using Domain;
using System;
using System.Collections.Generic;

namespace IntegrationTests.Helpers;

internal sealed class Utilities
{
    public static void InitializeDbForTests(LeanAuditorContext db)
    {
        var audit = Audit.Create(
            auditId: Guid.Parse("f285d889-27b2-4759-9d9f-0fffbdac982b"),
            author: "TestAuthor1",
            area: "TestArea1",
            startDate: DateTime.Parse("2022-11-14T14:02:42.000Z"),
            endDate: DateTime.Parse("2022-11-14T14:17:30.000Z")
        );

        var auditAction = AuditAction.Create(
            auditActionId: Guid.Parse("c1c5b6c9-e862-4adb-a2d1-80687617415b"),
            description: "TestAuditAction1"
        );

        auditAction.MarkComplete();

        audit.AddActions(auditAction);

        var questions = new List<Question>()
        {
            Question.Create(
                questionId: Guid.Parse("e611c40f-ac06-4d2f-9f95-9c56a98bdf66"),
                questionText: "TestQuestion1"
            ),
            Question.Create(
                questionId: Guid.Parse("699760e6-7f1e-4ac8-98d2-4011b746aa29"),
                questionText: "TestQuestion2"
            )
        };

        audit.AddAnswers([
            Answer.Create(
                answerId: Guid.Parse("469c66e1-88b9-4f18-ad5d-32bc4e58aae6"),
                questionId: questions[0].QuestionId,
                answerText: "1",
                answerType: "number"
            ),
            Answer.Create(
                answerId: Guid.Parse("fc966d94-f8f0-426c-8813-32146f1d6ecb"),
                questionId: questions[1].QuestionId,
                answerText: "5",
                answerType: "number"
            )
        ]);

        db.Audits.Add(audit);
        db.Questions.AddRange(questions);

        db.SaveChanges();
    }
}
