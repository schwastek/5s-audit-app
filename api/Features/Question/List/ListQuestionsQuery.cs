using Features.Question.Dto;
using MediatR;
using System.Collections.Generic;

namespace Features.Question.List;

public sealed record ListQuestionsQuery : IRequest<ListQuestionsQueryResult>;

public sealed record ListQuestionsQueryResult
{
    public required IReadOnlyList<QuestionDto> Questions { get; init; }
}
