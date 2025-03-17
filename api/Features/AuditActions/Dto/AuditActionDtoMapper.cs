using Features.Core.MappingService;
using Domain;

namespace Features.AuditActions.Dto;

public class AuditActionDtoMapper : IMapper<AuditAction, AuditActionDto>
{
    public AuditActionDto Map(AuditAction src)
    {
        return new AuditActionDto()
        {
            AuditActionId = src.AuditActionId,
            AuditId = src.AuditId,
            Description = src.Description,
            IsComplete = src.IsComplete
        };
    }
}
