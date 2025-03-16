using Features.Questions.Dto;
using MediatR;
using System.Collections.Generic;

namespace Features.Questions.List;

public sealed record ListQuestionsQuery : IRequest<ListQuestionsQueryResult>;

public sealed record ListQuestionsQueryResult
{
    public required IReadOnlyList<QuestionDto> Questions { get; init; }
}
