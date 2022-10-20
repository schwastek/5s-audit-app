using Api.Domain;
using Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace Api.Mappers
{
    public class AuditListMapper
    {
        public AuditListSingleDto Map(Audit audit)
        {
            var auditDto = new AuditListSingleDto()
            {
                AuditId = audit.AuditId,
                Area = audit.Area,
                Author = audit.Author,
                StartDate = audit.StartDate,
                EndDate = audit.EndDate,
                Score = audit.CalculateScore()
            };

            return auditDto;
        }
    }
}
