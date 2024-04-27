using Core.MappingService;
using System;

namespace Features.AuditAction.Dto;

public sealed record AuditActionForCreationDto
{
    public Guid AuditId { get; init; }
    public Guid ActionId { get; init; }
    public string Description { get; init; } = null!;
    public bool? IsComplete { get; set; }
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
