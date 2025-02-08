using Api.Requests.Answers.Dto;
using Api.Requests.AuditActions.Dto;
using System;
using System.Collections.Generic;

namespace Api.Requests.Audits.Get;

public sealed record GetAuditRequest
{
    /// <example>12aaf1bd-9aae-470d-8989-b991d8df8298</example>
    public Guid Id { get; set; }
}

public sealed record GetAuditResponse
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

    /// <example>0.6</example>
    public double Score { get; set; }

    public ICollection<AnswerDto> Answers { get; set; } = [];

    public ICollection<AuditActionDto> Actions { get; set; } = [];
}
