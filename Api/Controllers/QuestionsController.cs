using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Models;
using MediatR;
using Api.Queries;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ISender sender;

        public QuestionsController(ISender sender)
        {
            this.sender = sender;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<IActionResult> GetQuestions() 
        {
            List<QuestionDto> questionsDto = await sender.Send(new GetQuestionsQuery());

            return Ok(questionsDto);
        }
    }
}
