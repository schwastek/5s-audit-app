using Core.MappingService;
using System;

namespace Features.Question.Dto;

public sealed record QuestionDto
{
    public required Guid QuestionId { get; set; }
    public required string QuestionText { get; set; }
}

public class QuestionDtoMapper : IMapper<Domain.Question, QuestionDto>
{
    public QuestionDto Map(Domain.Question src)
    {
        return new QuestionDto()
        {
            QuestionId = src.QuestionId,
            QuestionText = src.QuestionText
        };
    }
}
