using System;

namespace Features.Answer.Dto;

public sealed record AnswerForCreationDto
{
    public required Guid QuestionId { get; init; }
    public required Guid AnswerId { get; init; }
    public required string AnswerType { get; init; }
    public required string AnswerText { get; init; }
}
