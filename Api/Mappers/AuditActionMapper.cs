using Api.Domain;
using Api.Models;

namespace Api.Mappers
{
    public class AuditActionMapper : IMapper<AuditActionForCreationDto, AuditAction>, IMapper<AuditAction, AuditActionDto>
    {
        public AuditAction Map(AuditActionForCreationDto auditActionDto)
        {
            AuditAction auditAction = new()
            {
                AuditId = auditActionDto.AuditId,
                AuditActionId = auditActionDto.ActionId,
                Description = auditActionDto.Description
            };

            return auditAction;
        }

        public AuditActionDto Map(AuditAction auditAction)
        {
            var auditActionDto = new AuditActionDto()
            {
                ActionId = auditAction.AuditActionId,
                AuditId = auditAction.AuditId,
                Description = auditAction.Description,
                IsComplete = auditAction.IsComplete
            };

            return auditActionDto;
        }
    }
}
