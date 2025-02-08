using Api.Constants;
using Api.Requests.Audits.Get;
using Api.Requests.Audits.List;
using Api.Requests.Audits.Save;
using Core.MappingService;
using Core.ValidatorService;
using Features.Audit.Get;
using Features.Audit.List;
using Features.Audit.Save;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace Api.Controllers;

[Route("api/audits")]
[ApiController]
[Produces(MediaTypeConstants.JsonContentType, MediaTypeConstants.ProblemDetailsContentType)]
public class AuditsController : ControllerBase
{
    private readonly IValidatorService validator;
    private readonly IMappingService mapper;
    private readonly ISender sender;

    public AuditsController(
        IValidatorService validator,
        IMappingService mapper,
        ISender sender
    )
    {
        this.validator = validator;
        this.mapper = mapper;
        this.sender = sender;
    }

    /// <summary>
    /// Gets list of audits
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ListAuditsResponse>> ListAudits([FromQuery] ListAuditsRequest request)
    {
        var query = mapper.Map<ListAuditsRequest, ListAuditsQuery>(request);
        var result = await sender.Send(query, HttpContext.RequestAborted);
        var response = mapper.Map<ListAuditsQueryResult, ListAuditsResponse>(result);

        return Ok(response);
    }

    /// <summary>
    /// Gets audit by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAuditResponse>> GetAudit([FromRoute] GetAuditRequest request)
    {
        var query = mapper.Map<GetAuditRequest, GetAuditQuery>(request);
        var result = await sender.Send(query, HttpContext.RequestAborted);
        var response = mapper.Map<GetAuditQueryResult, GetAuditResponse>(result);

        return Ok(response);
    }

    /// <summary>
    /// Creates a new audit
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<SaveAuditResponse>> SaveAudit([FromBody] SaveAuditRequest request)
    {
        var command = mapper.Map<SaveAuditRequest, SaveAuditCommand>(request);
        var result = await sender.Send(command, HttpContext.RequestAborted);
        var response = mapper.Map<SaveAuditCommandResult, SaveAuditResponse>(result);

        // Adds Location header that specifies the URI of the newly created item.
        return CreatedAtAction(nameof(GetAudit), new { id = response.AuditId }, response);
    }
}
