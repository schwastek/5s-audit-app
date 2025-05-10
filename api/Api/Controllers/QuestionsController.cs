using Api.Requests.Questions.List;
using Features.Core.MappingService;
using Features.Core.MediatorService;
using Features.Questions.List;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Api.Controllers;

[Authorize]
[Route("api/questions")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class QuestionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMappingService _mapper;

    public QuestionsController(IMediator mediator, IMappingService mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets the list of questions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ListQuestionsResponse>> ListQuestions()
    {
        var result = await _mediator.Send<ListQuestionsQuery, ListQuestionsQueryResult>(new ListQuestionsQuery(), HttpContext.RequestAborted);
        var response = _mapper.Map<ListQuestionsQueryResult, ListQuestionsResponse>(result);

        return Ok(response);
    }
}
