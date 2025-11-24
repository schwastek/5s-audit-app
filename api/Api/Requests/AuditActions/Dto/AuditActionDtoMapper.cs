using Infrastructure.MappingService;

namespace Api.Requests.AuditActions.Dto;

public sealed class AuditActionDtoMapper : IMapper<Features.AuditActions.Dto.AuditActionDto, Requests.AuditActions.Dto.AuditActionDto>
{
    public AuditActionDto Map(Features.AuditActions.Dto.AuditActionDto src)
    {
        return new AuditActionDto()
        {
            AuditActionId = src.AuditActionId,
            AuditId = src.AuditActionId,
            Description = src.Description,
            IsComplete = src.IsComplete,
            LastVersion = src.LastVersion
        };
    }
}
