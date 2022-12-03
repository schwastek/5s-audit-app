using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Models;
using MediatR;
using Api.Queries;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class QuestionsController : ControllerBase
    {
        private readonly ISender sender;

        public QuestionsController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Gets the list of questions
        /// </summary>
        /// <returns>A list of questions</returns>
        /// <response code="200">A list of questions</response>
        // GET: api/Questions
        [HttpGet]
        [ProducesResponseType(typeof(List<QuestionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetQuestions() 
        {
            List<QuestionDto> questionsDto = await sender.Send(new GetQuestionsQuery());

            return Ok(questionsDto);
        }
    }
}
