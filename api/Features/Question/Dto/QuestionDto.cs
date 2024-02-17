using Core.MappingService;
using System;

namespace Features.Question.Dto;

public sealed record QuestionDto
{
    public Guid QuestionId { get; set; }
    public string QuestionText { get; set; } = null!;
}

public class QuestionMapper :
    IMapper<Domain.Question, QuestionDto>,
    IMapper<QuestionDto, Api.Contracts.Question.Dto.QuestionDto>
{
    public QuestionDto Map(Domain.Question src)
    {
        return new QuestionDto()
        {
            QuestionId = src.QuestionId,
            QuestionText = src.QuestionText
        };
    }

    public Api.Contracts.Question.Dto.QuestionDto Map(QuestionDto src)
    {
        return new Api.Contracts.Question.Dto.QuestionDto()
        {
            QuestionId = src.QuestionId,
            QuestionText = src.QuestionText
        };
    }
}
