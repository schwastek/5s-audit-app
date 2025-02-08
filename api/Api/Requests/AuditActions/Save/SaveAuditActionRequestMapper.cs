using Core.MappingService;
using Features.AuditAction.Save;

namespace Api.Requests.AuditActions.Save;

public sealed class SaveAuditActionRequestMapper : IMapper<SaveAuditActionRequest, SaveAuditActionCommand>
{
    public SaveAuditActionCommand Map(SaveAuditActionRequest src)
    {
        return new SaveAuditActionCommand()
        {
            AuditId = src.AuditId,
            AuditActionId = src.AuditActionId,
            Description = src.Description
        };
    }
}

public sealed class SaveAuditActionCommandResultMapper : IMapper<SaveAuditActionCommandResult, SaveAuditActionResponse>
{
    public SaveAuditActionResponse Map(SaveAuditActionCommandResult src)
    {
        return new SaveAuditActionResponse()
        {
            AuditActionId = src.AuditActionId,
            AuditId = src.AuditActionId,
            Description = src.Description,
            IsComplete = src.IsComplete
        };
    }
}
