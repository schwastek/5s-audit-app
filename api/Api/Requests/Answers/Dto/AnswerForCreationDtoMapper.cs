using Core.MappingService;

namespace Api.Requests.Answers.Dto;

public sealed class AnswerForCreationDtoMapper : IMapper<AnswerForCreationDto, Features.Answer.Dto.AnswerForCreationDto>
{
    public Features.Answer.Dto.AnswerForCreationDto Map(AnswerForCreationDto src)
    {
        return new Features.Answer.Dto.AnswerForCreationDto()
        {
            AnswerId = src.AnswerId,
            AnswerText = src.AnswerText,
            AnswerType = src.AnswerType,
            QuestionId = src.QuestionId
        };
    }
}
