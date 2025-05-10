using Features.Questions.Dto;
using System.Collections.Generic;

namespace Features.Questions.List;

public sealed record ListQuestionsQuery;

public sealed record ListQuestionsQueryResult
{
    public required IReadOnlyList<QuestionDto> Questions { get; init; }
}
