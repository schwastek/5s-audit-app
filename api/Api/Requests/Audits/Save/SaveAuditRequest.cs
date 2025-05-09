﻿using Api.Requests.Answers.Dto;
using Api.Requests.AuditActions.Dto;
using System;
using System.Collections.Generic;

namespace Api.Requests.Audits.Save;

public sealed record SaveAuditRequest
{
    /// <example>d0a5a4ec-7b84-48c7-8028-133f1dd74b06</example>
    public Guid AuditId { get; set; }

    /// <example>John</example>
    public string Author { get; set; } = string.Empty;

    /// <example>Warehouse</example>
    public string Area { get; set; } = string.Empty;

    /// <example>2021-07-19T11:09:34.543Z</example>
    public DateTime StartDate { get; set; }

    /// <example>2021-07-19T11:09:44.543Z</example>
    public DateTime EndDate { get; set; }

    public ICollection<AnswerForCreationDto> Answers { get; set; } = [];
    public ICollection<AuditActionForCreationDto> Actions { get; set; } = [];
}

public sealed record SaveAuditResponse
{
    /// <example>d0a5a4ec-7b84-48c7-8028-133f1dd74b06</example>
    public Guid AuditId { get; set; }
}
