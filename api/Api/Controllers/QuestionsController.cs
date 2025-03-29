using Api.Requests.Questions.List;
using Features.Core.MappingService;
using Features.Questions.List;
using MediatR;
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
    private readonly ISender _sender;
    private readonly IMappingService _mapper;

    public QuestionsController(ISender sender, IMappingService mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets the list of questions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ListQuestionsResponse>> ListQuestions()
    {
        var result = await _sender.Send(new ListQuestionsQuery(), HttpContext.RequestAborted);
        var response = _mapper.Map<ListQuestionsQueryResult, ListQuestionsResponse>(result);

        return Ok(response);
    }
}
