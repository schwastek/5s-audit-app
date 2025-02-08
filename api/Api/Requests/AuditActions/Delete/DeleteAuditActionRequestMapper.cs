using Core.MappingService;
using Features.AuditAction.Delete;

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
