using Core.MappingService;

namespace Api.Requests.Questions.Dto;

public sealed class QuestionDtoMapper : IMapper<Features.Question.Dto.QuestionDto, QuestionDto>
{
    public QuestionDto Map(Features.Question.Dto.QuestionDto src)
    {
        return new QuestionDto()
        {
            QuestionId = src.QuestionId,
            QuestionText = src.QuestionText
        };
    }
}
