using Core.MappingService;

namespace Api.Requests.AuditActions.Dto;

public sealed class AuditActionForCreationDtoMapper : IMapper<AuditActionForCreationDto, Features.AuditAction.Dto.AuditActionForCreationDto>
{
    public Features.AuditAction.Dto.AuditActionForCreationDto Map(AuditActionForCreationDto src)
    {
        return new Features.AuditAction.Dto.AuditActionForCreationDto()
        {
            AuditActionId = src.AuditActionId,
            Description = src.Description
        };
    }
}
