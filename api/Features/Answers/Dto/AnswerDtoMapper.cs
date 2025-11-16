using Domain;
using Infrastructure.MappingService;

namespace Features.Answers.Dto;

public sealed class AnswerDtoMapper : IMapper<Answer, AnswerDto>
{
    public AnswerDto Map(Answer src)
    {
        return new AnswerDto()
        {
            AnswerId = src.AnswerId,
            AnswerText = src.AnswerText,
            AnswerType = src.AnswerType,
            QuestionId = src.QuestionId,
            QuestionText = src.Question!.QuestionText
        };
    }
}
