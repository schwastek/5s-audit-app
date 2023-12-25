using Api.Exceptions;
using Api.Mappers;
using Api.Models;
using Api.Queries;
using Api.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Api.Controllers;

[Route("api/audits")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class AuditsController : ControllerBase
{
    private readonly ISender sender;
    private readonly IMappingService mapper;

    public AuditsController(ISender sender, IMappingService mapper)
    {
        this.sender = sender;
        this.mapper = mapper;
    }

    /// <summary>
    /// Gets list of audits
    /// </summary>
    /// <param name="request"></param>
    /// <returns>A list of audits</returns>
    /// <response code="200">A list of audits</response>
    /// <response code="400">Validation error</response>
    // GET: api/audits
    [HttpGet(Name = nameof(GetAudits))]
    [ProducesResponseType(typeof(GetAuditsQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAudits([FromQuery] GetAuditsRequest request)
    {
        var query = mapper.Map<GetAuditsRequest, GetAuditsQuery>(request);
        var pagedResult = await sender.Send(query);

        return Ok(pagedResult);
    }

    /// <summary>
    /// Gets audit by ID
    /// </summary>
    /// <param name="id" example="f4940d26-7c0a-4ab6-b1cd-da8f708c5819"></param>
    /// <returns>A newly created audit</returns>
    /// <response code="200">A newly created audit</response>
    /// <response code="404">Audit is not found</response>
    // GET: api/Audits/5
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AuditDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAudit([FromRoute] Guid id)
    {
        AuditDto auditDto = await sender.Send(new GetAuditQuery(id));

        return Ok(auditDto);
    }

    /// <summary>
    /// Creates a new audit
    /// </summary>
    /// <param name="audit"></param>
    /// <returns>A newly created audit</returns>
    /// <response code="201">A newly created audit</response>
    // POST: api/Audits
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [ProducesResponseType(typeof(AuditDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> PostAudit([FromBody] AuditForCreationDto audit)
    {
        AuditDto auditDto = await sender.Send(new CreateAuditCommand(audit));

        // Adds a Location header to the response.
        // The Location header specifies the URI of the newly created item.
        return CreatedAtAction(nameof(GetAudit), new { id = auditDto.AuditId }, auditDto);
    }
}
