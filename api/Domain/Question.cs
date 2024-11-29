using System;

namespace Domain;

public class Question
{
    public Guid QuestionId { get; private set; }
    public string QuestionText { get; private set; }

    // EF Core calls this constructor when creating an instance of the entity.
    private Question(Guid questionId, string questionText)
    {
        QuestionId = questionId;
        QuestionText = questionText;
    }

    public static Question Create(Guid questionId, string questionText)
    {
        return new Question(questionId, questionText);
    }
}
