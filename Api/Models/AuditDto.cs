using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class AuditDto
    {
        public Guid AuditId { get; set; }
        public string Author { get; set; }
        public string Area { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Score { get; set; }
        public ICollection<AnswerDto> Answers { get; set; }
        public ICollection<AuditActionDto> Actions { get; set; }
    }
}
