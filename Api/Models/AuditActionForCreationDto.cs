using System;

namespace Api.Models
{
    public class AuditActionForCreationDto
    {
        public Guid AuditId { get; set; }
        public Guid ActionId { get; set; }
        public string Description { get; set; }
    }
}
