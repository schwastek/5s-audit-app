using System;
using System.ComponentModel.DataAnnotations;

namespace Domain;

public class AuditAction
{
    public Guid AuditActionId { get; set; }

    [Required]
    [MaxLength(280)]
    public string Description { get; set; }

    [Required]
    public bool IsComplete { get; set; }

    public Guid AuditId { get; set; }
}
