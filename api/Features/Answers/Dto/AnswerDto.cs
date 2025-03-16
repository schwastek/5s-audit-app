using System;

namespace Features.Answers.Dto;

public sealed record AnswerDto
{
    public required Guid AnswerId { get; init; }
    public required Guid QuestionId { get; init; }
    public required string QuestionText { get; set; }
    public required string AnswerType { get; set; }
    public required string AnswerText { get; set; }
}
