using Core.MappingService;
using System;

namespace Features.Answer.Dto;

public sealed record AnswerForCreationDto
{
    public required Guid QuestionId { get; init; }
    public required Guid AnswerId { get; init; }
    public required string AnswerType { get; init; }
    public required string AnswerText { get; init; }
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
