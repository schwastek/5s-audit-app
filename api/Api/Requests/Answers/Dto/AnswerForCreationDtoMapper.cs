using Infrastructure.MappingService;

namespace Api.Requests.Answers.Dto;

public sealed class AnswerForCreationDtoMapper : IMapper<Requests.Answers.Dto.AnswerForCreationDto, Features.Answers.Dto.AnswerForCreationDto>
{
    public Features.Answers.Dto.AnswerForCreationDto Map(AnswerForCreationDto src)
    {
        return new Features.Answers.Dto.AnswerForCreationDto()
        {
            AnswerId = src.AnswerId,
            AnswerText = src.AnswerText,
            AnswerType = src.AnswerType,
            QuestionId = src.QuestionId
        };
    }
}
