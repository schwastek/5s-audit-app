using System;

namespace Api.Models
{
    public class AuditListSingleDto
    {
        public Guid AuditId { get; set; }
        public string Author { get; set; }
        public string Area { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Score { get; set; }
    }
}
