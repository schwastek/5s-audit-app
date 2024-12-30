using Core.MappingService;
using System;

namespace Features.AuditAction.Dto;

public sealed record AuditActionForCreationDto
{
    public required Guid AuditActionId { get; init; }
    public required string Description { get; init; }
}

public class AuditActionForCreationDtoMapper :
    IMapper<Api.Contracts.AuditAction.Dto.AuditActionForCreationDto, AuditActionForCreationDto>
{
    public AuditActionForCreationDto Map(Api.Contracts.AuditAction.Dto.AuditActionForCreationDto src)
    {
        return new AuditActionForCreationDto()
        {
            AuditActionId = src.AuditActionId,
            Description = src.Description
        };
    }
}
