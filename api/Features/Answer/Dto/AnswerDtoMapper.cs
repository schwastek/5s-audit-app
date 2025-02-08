using Core.MappingService;

namespace Features.Answer.Dto;

public sealed class AnswerDtoMapper : IMapper<Domain.Answer, AnswerDto>
{
    public AnswerDto Map(Domain.Answer src)
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
