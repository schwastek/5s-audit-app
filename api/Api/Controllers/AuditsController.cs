using Api.Contracts.Audit.Requests;
using Api.Contracts.Common;
using Api.Mappers.MappingService;
using Features.Audit.Get;
using Features.Audit.List;
using Features.Audit.Save;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Api.Controllers;

[Route("api/audits")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class AuditsController : ControllerBase
{
    private readonly IMappingService mapper;
    private readonly ISender sender;

    public AuditsController(
        IMappingService mapper,
        ISender sender
    )
    {
        this.mapper = mapper;
        this.sender = sender;
    }

    /// <summary>
    /// Gets list of audits
    /// </summary>
    [HttpGet(Name = nameof(ListAudits))]
    [ProducesResponseType(typeof(ListAuditsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListAudits([FromQuery] ListAuditsRequest request)
    {
        var query = mapper.Map<ListAuditsRequest, ListAuditsQuery>(request);
        var result = await sender.Send(query);
        var response = mapper.Map<ListAuditsQueryResult, ListAuditsResponse>(result);

        return Ok(response);
    }

    /// <summary>
    /// Gets audit by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetAuditResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAudit([FromRoute] GetAuditRequest request)
    {
        var query = mapper.Map<GetAuditRequest, GetAuditQuery>(request);
        var result = await sender.Send(query);
        var response = mapper.Map<GetAuditQueryResult, GetAuditResponse>(result);

        return Ok(response);
    }

    /// <summary>
    /// Creates a new audit
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SaveAuditResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> SaveAudit([FromBody] SaveAuditRequest request)
    {
        var command = mapper.Map<SaveAuditRequest, SaveAuditCommand>(request);
        var result = await sender.Send(command);
        var response = mapper.Map<SaveAuditCommandResult, SaveAuditResponse>(result);

        // Adds a Location header to the response.
        // The Location header specifies the URI of the newly created item.
        return CreatedAtAction(nameof(GetAudit), new { id = result.Audit.AuditId }, result.Audit);
    }
}
