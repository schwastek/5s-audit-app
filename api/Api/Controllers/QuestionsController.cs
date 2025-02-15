using Api.Constants;
using Api.Exceptions;
using Api.Requests.Questions.List;
using Core.MappingService;
using Features.Question.List;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers;

[Route("api/questions")]
[ApiController]
[ProducesResponseType<CustomValidationProblemDetails>(StatusCodes.Status400BadRequest, MediaTypeConstants.ProblemDetailsContentType)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeConstants.ProblemDetailsContentType)]
[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
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
    [HttpGet]
    [ProducesResponseType<ListQuestionsResponse>(StatusCodes.Status200OK, MediaTypeConstants.JsonContentType)]
    public async Task<ActionResult<ListQuestionsResponse>> ListQuestions()
    {
        var result = await sender.Send(new ListQuestionsQuery(), HttpContext.RequestAborted);
        var response = mapper.Map<ListQuestionsQueryResult, ListQuestionsResponse>(result);

        return Ok(response);
    }
}
