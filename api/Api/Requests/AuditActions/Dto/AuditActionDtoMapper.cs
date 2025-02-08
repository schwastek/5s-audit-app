using Core.MappingService;

namespace Api.Requests.AuditActions.Dto;

public sealed class AuditActionDtoMapper : IMapper<Features.AuditAction.Dto.AuditActionDto, AuditActionDto>
{
    public AuditActionDto Map(Features.AuditAction.Dto.AuditActionDto src)
    {
        return new AuditActionDto()
        {
            AuditActionId = src.AuditActionId,
            AuditId = src.AuditActionId,
            Description = src.Description,
            IsComplete = src.IsComplete
        };
    }
}
