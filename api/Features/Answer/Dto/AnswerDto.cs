using Core.MappingService;
using System;

namespace Features.Answer.Dto;

public sealed record AnswerDto
{
    public Guid AnswerId { get; init; }
    public Guid QuestionId { get; init; }
    public string QuestionText { get; set; } = null!;
    public string AnswerType { get; set; } = null!;
    public string AnswerText { get; set; } = null!;
}

public class AnswerDtoMapper :
    IMapper<Domain.Answer, AnswerDto>,
    IMapper<AnswerDto, Api.Contracts.Answer.Dto.AnswerDto>
{
    public AnswerDto Map(Domain.Answer src)
    {
        return new AnswerDto()
        {
            AnswerId = src.AnswerId,
            AnswerText = src.AnswerText,
            AnswerType = src.AnswerType,
            QuestionId = src.QuestionId,
            QuestionText = src.Question.QuestionText
        };
    }

    public Api.Contracts.Answer.Dto.AnswerDto Map(AnswerDto src)
    {
        return new Api.Contracts.Answer.Dto.AnswerDto()
        {
            AnswerId = src.AnswerId,
            AnswerText = src.AnswerText,
            AnswerType = src.AnswerType,
            QuestionId = src.QuestionId,
            QuestionText = src.QuestionText
        };
    }
}
