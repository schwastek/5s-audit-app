using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.DbContexts;
using Api.Domain;
using Api.Mappers;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly LeanAuditorContext _context;
        private readonly IMappingService _mapper;

        public QuestionsController(LeanAuditorContext context, IMappingService mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<List<QuestionDto>>> GetQuestions() {
        
            List<Question> questions = await _context.Questions.ToListAsync();

            // Mapping
            List<QuestionDto> response = questions.Select(q => _mapper.Map<Question, QuestionDto>(q)).ToList();

            return Ok(response);
        }
    }
}
