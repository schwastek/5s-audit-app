﻿using Core.MappingService;
using System;

namespace Features.Question.Dto;

public sealed record QuestionDto
{
    public required Guid QuestionId { get; set; }
    public required string QuestionText { get; set; }
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
