using Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegrationTests.Helpers;

public static class TestDataService
{
    public static Audit BuildAudit(string? author = null, string? area = null)
    {
        author ??= TestValueGenerator.GenerateString();
        area ??= TestValueGenerator.GenerateString();

        var audit = Audit.Create(
            auditId: Guid.NewGuid(),
            author: author,
            area: area,
            startDate: DateTime.UtcNow,
            endDate: DateTime.UtcNow.AddMinutes(15)
        );

        return audit;
    }

    public static Audit BuildAudit(ICollection<Question> questions, string? author = null, string? area = null)
    {
        var audit = BuildAudit(author, area);
        var answers = BuildAnswersFor(questions);
        audit.AddAnswers(answers);

        return audit;
    }

    public static Question BuildQuestion()
    {
        var question = Question.Create(
            questionId: Guid.NewGuid(),
            questionText: TestValueGenerator.GenerateString()
        );

        return question;
    }

    public static ICollection<Question> BuildQuestions(int count = 5)
    {
        var questions = new List<Question>(count);

        for (int i = 0; i < count; i++)
        {
            var question = BuildQuestion();
            questions.Add(question);
        }

        return questions;
    }

    public static Answer BuildAnswerFor(Question question)
    {
        var answer = Answer.Create(
            answerId: Guid.NewGuid(),
            questionId: question.QuestionId,
            answerText: TestValueGenerator.GenerateInt().ToString(),
            answerType: "number"
        );

        return answer;
    }

    public static ICollection<Answer> BuildAnswersFor(IEnumerable<Question> questions)
    {
        var answers = new List<Answer>(questions.Count());

        foreach (var question in questions)
        {
            var answer = BuildAnswerFor(question);
            answers.Add(answer);
        }

        return answers;
    }

    public static AuditAction BuildAuditAction()
    {
        var auditAction = AuditAction.Create(
            auditActionId: Guid.NewGuid(),
            description: TestValueGenerator.GenerateString()
        );

        return auditAction;
    }
}
