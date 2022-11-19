using Api.Commands;
using Api.Domain;
using Api.Models;
using Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/actions")]
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly ISender sender;

        public ActionsController(ISender sender)
        {
            this.sender = sender;
        }

        // POST: api/actions
        [HttpPost]
        public async Task<IActionResult> CreateAction([FromBody] AuditActionForCreationDto auditAction)
        {
            AuditActionDto createdAuditAction = await sender.Send(new CreateAuditActionCommand(auditAction));

            return Ok(createdAuditAction);
        }

        // DELETE: api/actions/3
        [HttpDelete("{actionId:guid}")]
        public async Task<IActionResult> DeleteAction([FromRoute] Guid actionId)
        {
            await sender.Send(new DeleteAuditActionCommand(actionId));

            return NoContent();
        }

        // PUT: api/actions/3
        [HttpPut("{actionId:guid}")]
        public async Task<IActionResult> UpdateAction([FromRoute] Guid actionId,
            [FromBody] AuditActionForUpdateDto auditActionDto)
        {
            await sender.Send(new UpdateAuditActionCommand(actionId, auditActionDto));

            return NoContent();
        }
    }
}
