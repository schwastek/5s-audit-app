using Features.Core.MappingService;

namespace Api.Requests.AuditActions.Dto;

public sealed class AuditActionForCreationDtoMapper : IMapper<Requests.AuditActions.Dto.AuditActionForCreationDto, Features.AuditActions.Dto.AuditActionForCreationDto>
{
    public Features.AuditActions.Dto.AuditActionForCreationDto Map(AuditActionForCreationDto src)
    {
        return new Features.AuditActions.Dto.AuditActionForCreationDto()
        {
            AuditActionId = src.AuditActionId,
            Description = src.Description
        };
    }
}
