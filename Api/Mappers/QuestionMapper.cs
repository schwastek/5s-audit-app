using Api.Domain;
using Api.Models;

namespace Api.Mappers
{
    public class QuestionMapper : IMapper<Question, QuestionDto>
    {
        public QuestionDto Map(Question question)
        {
            QuestionDto questionDto = new()
            {
                QuestionId = question.QuestionId,
                QuestionText = question.QuestionText
            };

            return questionDto;
        }
    }
}
