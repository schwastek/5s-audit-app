using Features.AuditActions.Update;
using Infrastructure.MappingService;

namespace Api.Requests.AuditActions.Update;

public sealed class UpdateAuditActionRequestMapper : IMapper<UpdateAuditActionRequest, UpdateAuditActionCommand>
{
    public UpdateAuditActionCommand Map(UpdateAuditActionRequest src)
    {
        return new UpdateAuditActionCommand()
        {
            AuditActionId = src.AuditActionId,
            Description = src.Description,
            IsComplete = src.IsComplete,
            LastVersion = src.LastVersion
        };
    }
}

public sealed class UpdateAuditActionCommandResultMapper : IMapper<UpdateAuditActionCommandResult, UpdateAuditActionResponse>
{
    public UpdateAuditActionResponse Map(UpdateAuditActionCommandResult src)
    {
        return new UpdateAuditActionResponse()
        {
            AuditActionId = src.AuditActionId,
            Description = src.Description,
            IsComplete = src.IsComplete,
            LastVersion = src.LastVersion
        };
    }
}
