using Core.MappingService;

namespace Api.Requests.Answers.Dto;

public sealed class AnswerDtoMapper : IMapper<Features.Answer.Dto.AnswerDto, AnswerDto>
{
    public AnswerDto Map(Features.Answer.Dto.AnswerDto src)
    {
        return new AnswerDto()
        {
            AnswerId = src.AnswerId,
            AnswerText = src.AnswerText,
            AnswerType = src.AnswerType,
            QuestionId = src.QuestionId,
            QuestionText = src.QuestionText
        };
    }
}
