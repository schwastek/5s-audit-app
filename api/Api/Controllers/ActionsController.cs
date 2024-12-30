using Api.Contracts.AuditAction.Requests;
using Api.Contracts.Common;
using Core.MappingService;
using Features.AuditAction.Delete;
using Features.AuditAction.Save;
using Features.AuditAction.Update;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Api.Controllers;

[Route("api/actions")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class ActionsController : ControllerBase
{
    private readonly ISender sender;
    private readonly IMappingService mapper;

    public ActionsController(
        ISender sender,
        IMappingService mapper
    )
    {
        this.sender = sender;
        this.mapper = mapper;
    }

    /// <summary>
    /// Adds an action to the audit
    /// </summary>
    /// <param name="request"></param>
    /// <returns>A newly created action</returns>
    /// <response code="200">The action for the audit has been created</response>
    /// <response code="404">Audit is not found</response>
    [HttpPost]
    [ProducesResponseType(typeof(SaveAuditActionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SaveAuditAction([FromBody] SaveAuditActionRequest request)
    {
        var command = mapper.Map<SaveAuditActionRequest, SaveAuditActionCommand>(request);
        var result = await sender.Send(command);
        var response = mapper.Map<SaveAuditActionCommandResult, SaveAuditActionResponse>(result);

        return Ok(response);
    }

    /// <summary>
    /// Deletes an action
    /// </summary>
    /// <param name="auditActionId" example="ac1a0251-46cf-452b-9911-cfc998ea41a9"></param>
    /// <response code="204">The action has been deleted</response>
    /// <response code="404">The action is not found</response>
    [HttpDelete("{auditActionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditAction([FromRoute] Guid auditActionId)
    {
        var command = new DeleteAuditActionCommand
        {
            AuditActionId = auditActionId
        };
        await sender.Send(command);

        return NoContent();
    }

    /// <summary>
    /// Updates an action
    /// </summary>
    /// <param name="auditActionId" example="ac1a0251-46cf-452b-9911-cfc998ea41a9"></param>
    /// <param name="request"></param>
    /// <response code="204">The action has been updated</response>
    /// <response code="404">The action is not found</response>
    [HttpPut("{auditActionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAuditAction([FromRoute] Guid auditActionId, [FromBody] UpdateAuditActionRequest request)
    {
        var command = new UpdateAuditActionCommand()
        {
            AuditActionId = auditActionId,
            Description = request.Description,
            IsComplete = request.IsComplete
        };
        await sender.Send(command);

        return NoContent();
    }
}
