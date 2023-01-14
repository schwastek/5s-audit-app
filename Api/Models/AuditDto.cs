using System;
using System.Collections.Generic;

namespace Api.Models;

public class AuditDto
{
    /// <example>d0a5a4ec-7b84-48c7-8028-133f1dd74b06</example>
    public Guid AuditId { get; set; }
    
    /// <example>John</example>
    public string Author { get; set; }
    
    /// <example>Warehouse</example>
    public string Area { get; set; }
    
    /// <example>2021-07-19T11:09:34.543Z</example>
    public DateTime StartDate { get; set; }
    
    /// <example>2021-07-19T11:09:44.543Z</example>
    public DateTime EndDate { get; set; }
    
    /// <example>0.6</example>
    public double Score { get; set; }
    
    public ICollection<AnswerDto> Answers { get; set; }
    
    public ICollection<AuditActionDto> Actions { get; set; }
}
