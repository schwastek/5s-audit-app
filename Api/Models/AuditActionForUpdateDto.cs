using System;

namespace Api.Models
{
    public class AuditActionForUpdateDto
    {
        public string Description { get; set; }
        public bool IsComplete { get; set; }
    }
}
