using Api.Constants;
using Api.Exceptions;
using Api.Requests.AuditActions.Delete;
using Api.Requests.AuditActions.Save;
using Api.Requests.AuditActions.Update;
using Core.MappingService;
using Features.AuditActions.Delete;
using Features.AuditActions.Save;
using Features.AuditActions.Update;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers;

[Route("api/actions")]
[ApiController]
[ProducesResponseType<CustomValidationProblemDetails>(StatusCodes.Status400BadRequest, MediaTypeConstants.ProblemDetailsContentType)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeConstants.ProblemDetailsContentType)]
[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
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
    [HttpPost]
    [ProducesResponseType<SaveAuditActionResponse>(StatusCodes.Status200OK, MediaTypeConstants.JsonContentType)]
    public async Task<ActionResult<SaveAuditActionResponse>> SaveAuditAction([FromBody] SaveAuditActionRequest request)
    {
        var command = mapper.Map<SaveAuditActionRequest, SaveAuditActionCommand>(request);
        var result = await sender.Send(command, HttpContext.RequestAborted);
        var response = mapper.Map<SaveAuditActionCommandResult, SaveAuditActionResponse>(result);

        return Ok(response);
    }

    /// <summary>
    /// Deletes an action
    /// </summary>
    [HttpDelete("{auditActionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAuditAction([FromRoute] DeleteAuditActionRequest request)
    {
        var command = mapper.Map<DeleteAuditActionRequest, DeleteAuditActionCommand>(request);
        await sender.Send(command, HttpContext.RequestAborted);

        return NoContent();
    }

    /// <summary>
    /// Updates an action
    /// </summary>
    [HttpPut("{auditActionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateAuditAction([FromRoute] Guid auditActionId, [FromBody] UpdateAuditActionRequest request)
    {
        request.AuditActionId = auditActionId;
        var command = mapper.Map<UpdateAuditActionRequest, UpdateAuditActionCommand>(request);
        await sender.Send(command, HttpContext.RequestAborted);

        return NoContent();
    }
}
