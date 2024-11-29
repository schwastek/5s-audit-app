using System;

namespace Domain;

public class Answer
{
    public Guid AnswerId { get; private set; }
    public string AnswerText { get; private set; }
    public string AnswerType { get; private set; }

    public Guid AuditId { get; private set; }
    public Guid QuestionId { get; private set; }

    // Required relationships still need nullable navigation properties,
    // because they it may or may not be loaded by a particular query.
    // See: https://learn.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types#required-navigation-properties
    public Question? Question { get; private set; }

    // EF Core calls this constructor when creating an instance of the entity.
    private Answer(Guid answerId, string answerText, string answerType)
    {
        AnswerId = answerId;
        AnswerText = answerText;
        AnswerType = answerType;
    }

    public static Answer Create(Guid answerId, Guid questionId, string answerText, string answerType)
    {
        var answer = new Answer(answerId, answerText, answerType);
        answer.QuestionId = questionId;

        return answer;
    }
}
