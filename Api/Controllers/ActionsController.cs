using Api.Commands;
using Api.Exceptions;
using Api.Models;
using Api.Queries;
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

    public ActionsController(ISender sender)
    {
        this.sender = sender;
    }

    /// <summary>
    /// Adds an action to the audit
    /// </summary>
    /// <param name="auditAction"></param>
    /// <returns>A newly created action</returns>
    /// <response code="200">The action for the audit has been created</response>
    /// <response code="404">Audit is not found</response>
    // POST: api/actions
    [HttpPost]
    [ProducesResponseType(typeof(AuditActionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAction([FromBody] AuditActionForCreationDto auditAction)
    {
        AuditActionDto createdAuditAction = await sender.Send(new CreateAuditActionCommand(auditAction));

        return Ok(createdAuditAction);
    }

    /// <summary>
    /// Deletes an action
    /// </summary>
    /// <param name="actionId" example="ac1a0251-46cf-452b-9911-cfc998ea41a9"></param>
    /// <response code="204">The action has been deleted</response>
    /// <response code="404">The action is not found</response>
    // DELETE: api/actions/3
    [HttpDelete("{actionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAction([FromRoute] Guid actionId)
    {
        await sender.Send(new DeleteAuditActionCommand(actionId));

        return NoContent();
    }

    /// <summary>
    /// Updates an action
    /// </summary>
    /// <param name="actionId" example="ac1a0251-46cf-452b-9911-cfc998ea41a9"></param>
    /// <param name="auditActionDto"></param>
    /// <response code="204">The action has been updated</response>
    /// <response code="404">The action is not found</response>
    // PUT: api/actions/3
    [HttpPut("{actionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAction(
        [FromRoute] Guid actionId, 
        [FromBody] AuditActionForUpdateDto auditActionDto
        )
    {
        await sender.Send(new UpdateAuditActionCommand(actionId, auditActionDto));

        return NoContent();
    }
}
