using Core.MappingService;

namespace Features.AuditAction.Dto;

public class AuditActionDtoMapper : IMapper<Domain.AuditAction, AuditActionDto>
{
    public AuditActionDto Map(Domain.AuditAction src)
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
