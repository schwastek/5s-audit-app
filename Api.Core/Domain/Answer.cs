using System;

namespace Api.Core.Domain;

public class Answer
{
    public Guid AnswerId { get; set; }
    public string AnswerText { get; set; }
    public string AnswerType { get; set; }

    public Guid AuditId { get; set; }
    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
}
