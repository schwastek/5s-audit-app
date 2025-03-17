using Features.AuditActions.Delete;
using Features.Core.MappingService;

namespace Api.Requests.AuditActions.Delete;

public sealed class DeleteAuditActionRequestMapper : IMapper<DeleteAuditActionRequest, DeleteAuditActionCommand>
{
    public DeleteAuditActionCommand Map(DeleteAuditActionRequest src)
    {
        return new DeleteAuditActionCommand()
        {
            AuditActionId = src.AuditActionId
        };
    }
}
