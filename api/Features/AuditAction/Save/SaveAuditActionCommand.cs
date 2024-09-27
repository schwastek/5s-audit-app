using Api.Contracts.AuditAction.Requests;
using Core.MappingService;
using Features.AuditAction.Dto;
using MediatR;
using System;

namespace Features.AuditAction.Save;

public sealed record SaveAuditActionCommand : IRequest<SaveAuditActionCommandResult>
{
    public required Guid AuditId { get; init; }
    public required Guid ActionId { get; init; }
    public required string Description { get; init; }
    public required bool IsComplete { get; set; }
}

public sealed record SaveAuditActionCommandResult
{
    public required AuditActionDto AuditAction { get; init; }
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
        var auditAction = mapper.Map<AuditActionDto, Api.Contracts.AuditAction.Dto.AuditActionDto>(src.AuditAction);

        return new SaveAuditActionResponse()
        {
            AuditAction = auditAction
        };
    }
}
