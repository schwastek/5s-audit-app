using Api.Contracts.AuditAction.Requests;
using Core.MappingService;
using MediatR;
using System;

namespace Features.AuditAction.Update;

public sealed record UpdateAuditActionCommand : IRequest
{
    public Guid ActionId { get; set; }
    public string Description { get; init; } = null!;
    public bool IsComplete { get; set; }
}

public class UpdateAuditActionCommandMapper : IMapper<UpdateAuditActionRequest, UpdateAuditActionCommand>
{
    public UpdateAuditActionCommand Map(UpdateAuditActionRequest src)
    {
        return new UpdateAuditActionCommand()
        {
            ActionId = src.ActionId,
            Description = src.Description,
            IsComplete = src.IsComplete
        };
    }
}
