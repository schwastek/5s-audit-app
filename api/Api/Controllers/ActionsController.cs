﻿using Api.Contracts.AuditAction.Requests;
using Api.Contracts.Common;
using Core.MappingService;
using Core.ValidatorService;
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
    private readonly IValidatorService validator;

    public ActionsController(
        ISender sender,
        IMappingService mapper,
        IValidatorService validator
    )
    {
        this.sender = sender;
        this.mapper = mapper;
        this.validator = validator;
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
        await validator.ValidateAndThrowAsync(request);
        var command = mapper.Map<SaveAuditActionRequest, SaveAuditActionCommand>(request);
        var result = await sender.Send(command);
        var response = mapper.Map<SaveAuditActionCommandResult, SaveAuditActionResponse>(result);

        return Ok(response);
    }

    /// <summary>
    /// Deletes an action
    /// </summary>
    /// <param name="actionId" example="ac1a0251-46cf-452b-9911-cfc998ea41a9"></param>
    /// <response code="204">The action has been deleted</response>
    /// <response code="404">The action is not found</response>
    [HttpDelete("{actionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuditAction([FromRoute] Guid actionId)
    {
        await sender.Send(new DeleteAuditActionCommand { ActionId = actionId });

        return NoContent();
    }

    /// <summary>
    /// Updates an action
    /// </summary>
    /// <param name="actionId" example="ac1a0251-46cf-452b-9911-cfc998ea41a9"></param>
    /// <param name="request"></param>
    /// <response code="204">The action has been updated</response>
    /// <response code="404">The action is not found</response>
    [HttpPut("{actionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAuditAction([FromRoute] Guid actionId, [FromBody] UpdateAuditActionRequest request)
    {
        request.ActionId = actionId;
        await validator.ValidateAndThrowAsync(request);
        var command = mapper.Map<UpdateAuditActionRequest, UpdateAuditActionCommand>(request);
        await sender.Send(command);

        return NoContent();
    }
}
