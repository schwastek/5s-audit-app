using System;

namespace Api.Core.Domain;

public class Question
{
    public Guid QuestionId { get; set; }
    public string QuestionText { get; set; }
}
