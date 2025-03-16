using Core.MappingService;

namespace Api.Requests.Answers.Dto;

public sealed class AnswerDtoMapper : IMapper<Features.Answers.Dto.AnswerDto, Requests.Answers.Dto.AnswerDto>
{
    public AnswerDto Map(Features.Answers.Dto.AnswerDto src)
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
