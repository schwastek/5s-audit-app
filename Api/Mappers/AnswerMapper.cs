using Api.Domain;
using Api.Models;

namespace Api.Mappers
{
    public class AnswerMapper : IMapper<AnswerForCreationDto, Answer>, IMapper<Answer, AnswerDto>
    {
        private readonly IMappingService _mapper;

        public AnswerMapper(IMappingService mapper)
        {
            _mapper = mapper;
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
            QuestionDto question = _mapper.Map<Question, QuestionDto>(answer.Question);

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
