using Api.DbContexts;
using Api.Domain;
using Api.Mappers;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/actions")]
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly LeanAuditorContext _context;
        private readonly AuditService _auditService;
        private readonly AuditActionMapper _mapper;

        public ActionsController(LeanAuditorContext context, AuditService auditService, AuditActionMapper mapper)
        {
            _context = context;
            _auditService = auditService;
            _mapper = mapper;
        }

        // POST: api/actions
        [HttpPost]
        public async Task<ActionResult<AuditAction>> CreateAction([FromBody] AuditActionForCreationDto auditActionDto)
        {
            if (!_auditService.AuditExists(auditActionDto.AuditId))
            {
                return NotFound();
            }

            // Map
            AuditAction auditAction = _mapper.Map(auditActionDto);

            // Add to DB
            _context.AuditActions.Add(auditAction);
            await _context.SaveChangesAsync();

            return Ok(auditAction);
        }

        // DELETE: api/actions/3
        [HttpDelete("{actionId:guid}")]
        public async Task<ActionResult<AuditAction>> DeleteAction([FromRoute] Guid actionId)
        {
            AuditAction auditAction = await _context.AuditActions.FindAsync(actionId);

            if (auditAction == null)
            {
                return NotFound(new { Message = $"Action with ID {actionId} does not exist." });
            }

            _context.Remove(auditAction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/actions/3
        [HttpPut("{actionId:guid}")]
        public async Task<ActionResult<AuditAction>> UpdateAction([FromRoute] Guid actionId,
            [FromBody] AuditActionForUpdateDto auditActionDto)
        {
            AuditAction auditAction = await _context.AuditActions.FindAsync(actionId);

            if (auditAction == null)
            {
                return NotFound(new { Message = $"Action with ID {actionId} does not exist." });
            }

            auditAction.Description = auditActionDto.Description ?? auditAction.Description;
            auditAction.IsComplete = auditActionDto.IsComplete;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
