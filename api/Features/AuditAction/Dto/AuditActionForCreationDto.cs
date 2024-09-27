using Core.MappingService;
using System;

namespace Features.AuditAction.Dto;

public sealed record AuditActionForCreationDto
{
    public required Guid AuditId { get; init; }
    public required Guid ActionId { get; init; }
    public required string Description { get; init; }
    public required bool IsComplete { get; set; }
}

public class AuditActionForCreationDtoMapper :
    IMapper<Api.Contracts.AuditAction.Dto.AuditActionForCreationDto, AuditActionForCreationDto>
{
    public AuditActionForCreationDto Map(Api.Contracts.AuditAction.Dto.AuditActionForCreationDto src)
    {
        return new AuditActionForCreationDto()
        {
            ActionId = src.ActionId,
            AuditId = src.AuditId,
            Description = src.Description,
            IsComplete = src.IsComplete
        };
    }
}
