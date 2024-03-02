using Core.MappingService;
using System;

namespace Features.AuditAction.Dto;

public sealed record AuditActionDto
{
    public Guid ActionId { get; set; }
    public Guid AuditId { get; set; }
    public string Description { get; set; } = null!;
    public bool IsComplete { get; set; }
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
