using Api.Requests.Audits.Get;
using Api.Requests.Audits.List;
using Api.Requests.Audits.Save;
using Features.Audits.Get;
using Features.Audits.List;
using Features.Audits.Save;
using Features.Core.MappingService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Api.Controllers;

[Authorize]
[Route("api/audits")]
[ApiController]
// Note #1:
// You cannot define multiple ProducesResponseType attributes with the same HTTP status code
// but different response types (e.g. HTTP 400 with both ProblemDetails and CustomValidationProblemDetails).
// See: https://github.com/dotnet/aspnetcore/issues/56177
// Note #2:
// The Produces attribute content type will overwrite the content type specified in the ProducesResponseType attribute.
// The Produces attribute takes precedence if both are defined.
// Note #3:
// The ProducesResponseType attribute requires a response type to set the correct content type.
// Note #4:
// Framework doesn't infer 200 OK from ActionResult<T> if other [ProducesResponseType] attributes exist.
// If [ProducesResponseType(typeof(void), 401)] is present, Swagger will document 401 but not 200 OK.
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Authorize]
public class AuditsController : ControllerBase
{
    private readonly IMappingService _mapper;
    private readonly ISender _sender;

    public AuditsController(
        IMappingService mapper,
        ISender sender
    )
    {
        _mapper = mapper;
        _sender = sender;
    }

    /// <summary>
    /// Gets list of audits
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ListAuditsResponse>> ListAudits([FromQuery] ListAuditsRequest request)
    {
        var query = _mapper.Map<ListAuditsRequest, ListAuditsQuery>(request);
        var result = await _sender.Send(query, HttpContext.RequestAborted);
        var response = _mapper.Map<ListAuditsQueryResult, ListAuditsResponse>(result);

        return Ok(response);
    }

    /// <summary>
    /// Gets audit by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetAuditResponse>> GetAudit([FromRoute] GetAuditRequest request)
    {
        var query = _mapper.Map<GetAuditRequest, GetAuditQuery>(request);
        var result = await _sender.Send(query, HttpContext.RequestAborted);
        var response = _mapper.Map<GetAuditQueryResult, GetAuditResponse>(result);

        return Ok(response);
    }

    /// <summary>
    /// Creates a new audit
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<SaveAuditResponse>> SaveAudit([FromBody] SaveAuditRequest request)
    {
        var command = _mapper.Map<SaveAuditRequest, SaveAuditCommand>(request);
        var result = await _sender.Send(command, HttpContext.RequestAborted);
        var response = _mapper.Map<SaveAuditCommandResult, SaveAuditResponse>(result);

        // Adds Location header that specifies the URI of the newly created item.
        return CreatedAtAction(nameof(GetAudit), new { id = response.AuditId }, response);
    }
}
