using Api.Contracts.Question.Requests;
using Core.MappingService;
using Features.Question.List;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Api.Controllers;

[Route("api/questions")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class QuestionsController : ControllerBase
{
    private readonly ISender sender;
    private readonly IMappingService mapper;

    public QuestionsController(ISender sender, IMappingService mapper)
    {
        this.sender = sender;
        this.mapper = mapper;
    }

    /// <summary>
    /// Gets the list of questions
    /// </summary>
    /// <returns>A list of questions</returns>
    /// <response code="200">A list of questions</response>
    [HttpGet]
    [ProducesResponseType(typeof(ListQuestionsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListQuestions()
    {
        var result = await sender.Send(new ListQuestionsQuery());
        var response = mapper.Map<ListQuestionsQueryResult, ListQuestionsResponse>(result);

        return Ok(response);
    }
}
