using Api.Contracts.AuditAction.Requests;
using Core.MappingService;
using MediatR;
using System;

namespace Features.AuditAction.Update;

public sealed record UpdateAuditActionCommand : IRequest
{
    public required Guid ActionId { get; set; }
    public required string Description { get; init; }
    public required bool IsComplete { get; set; }
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
