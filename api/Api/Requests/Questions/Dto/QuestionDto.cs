﻿using System;

namespace Api.Requests.Questions.Dto;

public sealed record QuestionDto
{
    /// <example>af70e9f3-9a70-4178-80dc-87d38bb1c810</example>
    public Guid QuestionId { get; set; }

    /// <example>Are all tools in the work area currently in use?</example>
    public string QuestionText { get; set; } = string.Empty;
}
