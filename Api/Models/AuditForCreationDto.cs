using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class AuditForCreationDto
    {
        public Guid AuditId { get; set; }
        public string Author { get; set; }
        public string Area { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<AnswerForCreationDto> Answers { get; set; }
        public ICollection<AuditActionForCreationDto> Actions { get; set; }
    }
}
