using System;

namespace Api.Models
{
    public class AuditActionDto
    {
        public Guid ActionId { get; set; }
        public Guid AuditId { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
    }
}
