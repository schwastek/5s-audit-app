using Core.MappingService;
using System;

namespace Features.AuditAction.Dto;

public sealed record AuditActionDto
{
    public required Guid ActionId { get; set; }
    public required Guid AuditId { get; set; }
    public required string Description { get; set; }
    public required bool IsComplete { get; set; }
}

public class AuditActionDtoMapper :
    IMapper<Domain.AuditAction, AuditActionDto>,
    IMapper<AuditActionDto, Api.Contracts.AuditAction.Dto.AuditActionDto>
{
    public AuditActionDto Map(Domain.AuditAction src)
    {
        return new AuditActionDto()
        {
            ActionId = src.AuditActionId,
            AuditId = src.AuditId,
            Description = src.Description,
            IsComplete = src.IsComplete
        };
    }

    public Api.Contracts.AuditAction.Dto.AuditActionDto Map(AuditActionDto src)
    {
        return new Api.Contracts.AuditAction.Dto.AuditActionDto()
        {
            ActionId = src.ActionId,
            AuditId = src.AuditId,
            Description = src.Description,
            IsComplete = src.IsComplete
        };
    }
}
