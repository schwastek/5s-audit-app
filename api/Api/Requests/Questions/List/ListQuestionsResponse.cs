using Api.Requests.Questions.Dto;
using System.Collections.Generic;

namespace Api.Requests.Questions.List;

public sealed record ListQuestionsResponse
{
    public ICollection<QuestionDto> Questions { get; set; } = [];
}
