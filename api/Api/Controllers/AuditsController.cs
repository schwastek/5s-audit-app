using Api.Constants;
using Api.Exceptions;
using Api.Requests.Audits.Get;
using Api.Requests.Audits.List;
using Api.Requests.Audits.Save;
using Features.Audits.Get;
using Features.Audits.List;
using Features.Audits.Save;
using Features.Core.MappingService;
using Features.Core.ValidatorService;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace Api.Controllers;

[Route("api/audits")]
[ApiController]
// Note #1:
// You cannot define multiple ProducesResponseType attributes with the same HTTP status code
// but different response types (e.g. HTTP 400 with both ProblemDetails and CustomValidationProblemDetails).
// See: https://github.com/dotnet/aspnetcore/issues/56177
// Note #2:
// The Produces attribute content type will overwrite the content type specified in the ProducesResponseType attribute.
// The Produces attribute takes precedence if both are defined. Use only ProducesResponseType.
// Note #3:
// The ProducesResponseType attribute requires a response type to set the correct content type.
// Note #4:
// Pass typeof(void) explicitly to the ProducesResponseType attribute to avoid
// the default ProblemDetails schema in the OpenAPI specification for responses with no content.
[ProducesResponseType<CustomValidationProblemDetails>(StatusCodes.Status400BadRequest, MediaTypeConstants.ProblemDetailsContentType)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeConstants.ProblemDetailsContentType)]
[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
public class AuditsController : ControllerBase
{
    private readonly IValidatorService _validator;
    private readonly IMappingService _mapper;
    private readonly ISender _sender;

    public AuditsController(
        IValidatorService validator,
        IMappingService mapper,
        ISender sender
    )
    {
        _validator = validator;
        _mapper = mapper;
        _sender = sender;
    }

    /// <summary>
    /// Gets list of audits
    /// </summary>
    [HttpGet]
    [ProducesResponseType<ListAuditsResponse>(StatusCodes.Status200OK, MediaTypeConstants.JsonContentType)]
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
    [ProducesResponseType<GetAuditResponse>(StatusCodes.Status200OK, MediaTypeConstants.JsonContentType)]
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
    [ProducesResponseType<SaveAuditResponse>(StatusCodes.Status201Created, MediaTypeConstants.JsonContentType)]
    public async Task<ActionResult<SaveAuditResponse>> SaveAudit([FromBody] SaveAuditRequest request)
    {
        var command = _mapper.Map<SaveAuditRequest, SaveAuditCommand>(request);
        var result = await _sender.Send(command, HttpContext.RequestAborted);
        var response = _mapper.Map<SaveAuditCommandResult, SaveAuditResponse>(result);

        // Adds Location header that specifies the URI of the newly created item.
        return CreatedAtAction(nameof(GetAudit), new { id = response.AuditId }, response);
    }
}
