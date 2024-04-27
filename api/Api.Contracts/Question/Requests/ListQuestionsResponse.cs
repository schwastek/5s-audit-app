using Api.Contracts.Question.Dto;
using System.Collections.Generic;

namespace Api.Contracts.Question.Requests
{
    public class ListQuestionsResponse
    {
        public IReadOnlyCollection<QuestionDto> Questions { get; set; }
    }
}
