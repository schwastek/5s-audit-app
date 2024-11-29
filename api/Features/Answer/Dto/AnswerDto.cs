using Core.MappingService;
using System;

namespace Features.Answer.Dto;

public sealed record AnswerDto
{
    public required Guid AnswerId { get; init; }
    public required Guid QuestionId { get; init; }
    public required string QuestionText { get; set; }
    public required string AnswerType { get; set; }
    public required string AnswerText { get; set; }
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
            QuestionText = src.Question!.QuestionText
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
