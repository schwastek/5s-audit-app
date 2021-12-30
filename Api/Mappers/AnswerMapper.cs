using Api.Domain;
using Api.Models;

namespace Api.Mappers
{
    public class AnswerMapper
    {
        private readonly QuestionMapper _questionMapper;

        public AnswerMapper(QuestionMapper questionMapper)
        {
            _questionMapper = questionMapper;
        }

        public Answer Map(AnswerForCreationDto answerDto)
        {
            Answer answer = new()
            {
                QuestionId = answerDto.QuestionId,
                AnswerId = answerDto.AnswerId,
                AnswerType = answerDto.AnswerType,
                AnswerText = answerDto.AnswerText,
            };

            return answer;
        }

        public AnswerDto Map(Answer answer)
        {
            QuestionDto question = _questionMapper.Map(answer.Question);

            AnswerDto answerDto = new()
            {
                AnswerId = answer.AnswerId,
                AnswerType = answer.AnswerType,
                AnswerText = answer.AnswerText,
                QuestionId = question.QuestionId,
                QuestionText = question.QuestionText
            };

            return answerDto;
        }
    }
}
