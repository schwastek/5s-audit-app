﻿using Features.AuditActions.Update;
using Features.Core.MappingService;

namespace Api.Requests.AuditActions.Update;

public sealed class UpdateAuditActionRequestMapper : IMapper<UpdateAuditActionRequest, UpdateAuditActionCommand>
{
    public UpdateAuditActionCommand Map(UpdateAuditActionRequest src)
    {
        return new UpdateAuditActionCommand()
        {
            AuditActionId = src.AuditActionId,
            Description = src.Description,
            IsComplete = src.IsComplete
        };
    }
}
