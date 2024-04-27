using Api.Contracts.AuditAction.Requests;
using Api.Mappers.MappingService;
using Core.MappingService;
using MediatR;
using System;

namespace Features.AuditAction.Save;

public sealed record SaveAuditActionCommand : IRequest<SaveAuditActionCommandResult>
{
    public Guid AuditId { get; init; }
    public Guid ActionId { get; init; }
    public string Description { get; init; } = null!;
    public bool IsComplete { get; set; }
}

public sealed record SaveAuditActionCommandResult
{
    public Dto.AuditActionDto AuditAction { get; init; } = null!;
}

public class SaveAuditActionCommandMapper :
    IMapper<SaveAuditActionRequest, SaveAuditActionCommand>,
    IMapper<SaveAuditActionCommandResult, SaveAuditActionResponse>
{
    private readonly IMappingService mapper;

    public SaveAuditActionCommandMapper(IMappingService mapper)
    {
        this.mapper = mapper;
    }

    public SaveAuditActionCommand Map(SaveAuditActionRequest src)
    {
        return new SaveAuditActionCommand()
        {
            AuditId = src.AuditId,
            ActionId = src.ActionId,
            Description = src.Description,
            IsComplete = src.IsComplete
        };
    }

    public SaveAuditActionResponse Map(SaveAuditActionCommandResult src)
    {
        var auditAction = mapper.Map<Dto.AuditActionDto, Api.Contracts.AuditAction.Dto.AuditActionDto>(src.AuditAction);

        return new SaveAuditActionResponse()
        {
            AuditAction = auditAction
        };
    }
}
