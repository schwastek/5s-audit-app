using Domain;
using Features.Core.MappingService;
using System;

namespace Features.Questions.Dto;

public sealed record QuestionDto
{
    public required Guid QuestionId { get; set; }
    public required string QuestionText { get; set; }
}

public class QuestionDtoMapper : IMapper<Question, QuestionDto>
{
    public QuestionDto Map(Question src)
    {
        return new QuestionDto()
        {
            QuestionId = src.QuestionId,
            QuestionText = src.QuestionText
        };
    }
}
