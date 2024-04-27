using Core.MappingService;
using System;

namespace Features.Answer.Dto;

public sealed record AnswerForCreationDto
{
    public Guid QuestionId { get; init; }
    public Guid AnswerId { get; init; }
    public string AnswerType { get; init; } = null!;
    public string AnswerText { get; init; } = null!;
}

public class AnswerForCreationDtoMapper :
    IMapper<Api.Contracts.Answer.Dto.AnswerForCreationDto, AnswerForCreationDto>
{
    public AnswerForCreationDto Map(Api.Contracts.Answer.Dto.AnswerForCreationDto src)
    {
        return new AnswerForCreationDto()
        {
            AnswerId = src.AnswerId,
            AnswerText = src.AnswerText,
            AnswerType = src.AnswerType,
            QuestionId = src.QuestionId
        };
    }
}
