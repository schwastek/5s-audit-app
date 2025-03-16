using Core.MappingService;

namespace Api.Requests.Questions.Dto;

public sealed class QuestionDtoMapper : IMapper<Features.Questions.Dto.QuestionDto, Requests.Questions.Dto.QuestionDto>
{
    public QuestionDto Map(Features.Questions.Dto.QuestionDto src)
    {
        return new QuestionDto()
        {
            QuestionId = src.QuestionId,
            QuestionText = src.QuestionText
        };
    }
}
