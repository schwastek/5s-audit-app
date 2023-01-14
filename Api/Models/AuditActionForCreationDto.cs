using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class AuditActionForCreationDto
{
    /// <example>f4940d26-7c0a-4ab6-b1cd-da8f708c5819</example>
    [Required]
    public Guid AuditId { get; set; }
    
    /// <example>ac1a0251-46cf-452b-9911-cfc998ea41a9</example>
    public Guid ActionId { get; set; }
    
    /// <example>Clean up</example>
    [Required]
    public string Description { get; set; }
}
