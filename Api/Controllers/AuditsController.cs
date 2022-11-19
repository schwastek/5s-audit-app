using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.ResourceParameters;
using Api.Helpers;
using Api.Extensions;
using Microsoft.AspNetCore.Routing;
using Api.Queries;
using MediatR;

namespace Api.Controllers
{
    [Route("api/audits")]
    [ApiController]
    public class AuditsController : ControllerBase
    {
        private readonly ISender sender;

        public AuditsController(ISender sender)
        {
            this.sender = sender;
        }

        // GET: api/audits
        [HttpGet(Name = nameof(GetAudits))]
        public async Task<IActionResult> GetAudits([FromQuery] AuditsUrlQueryParameters queryParameters)
        {
            var pagedResult = await sender.Send(new GetAuditsQuery(queryParameters));

            // Add pagination metadata
            string previousPageLink = pagedResult.metaData.HasPrevious ?
                CreateAuditsResourceUri(queryParameters, EResourceUriType.PreviousPage)
                : null;

            string nextPageLink = pagedResult.metaData.HasNext ?
                CreateAuditsResourceUri(queryParameters, EResourceUriType.NextPage)
                : null;

            Response.AddPaginationHeader(pagedResult.metaData, previousPageLink, nextPageLink);

            return Ok(pagedResult.audits);
        }

        // GET: api/Audits/5
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAudit([FromRoute] Guid id)
        {
            AuditDto auditDto = await sender.Send(new GetAuditQuery(id));

            return Ok(auditDto);
        }

        // POST: api/Audits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostAudit([FromBody] AuditForCreationDto audit)
        {
            AuditDto auditDto = await sender.Send(new CreateAuditCommand(audit));

            // Adds a Location header to the response.
            // The Location header specifies the URI of the newly created item.
            return CreatedAtAction(nameof(GetAudit), new { id = auditDto.AuditId }, auditDto);
        }

        private string CreateAuditsResourceUri(AuditsUrlQueryParameters auditsResourceParameters,
            EResourceUriType type)
        {
            switch (type)
            {
                case EResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetAudits),
                        new
                        {
                            pageSize = auditsResourceParameters.PageSize,
                            pageNumber = auditsResourceParameters.PageNumber - 1,
                            orderBy = auditsResourceParameters.OrderBy
                        });
                case EResourceUriType.NextPage:
                    return Url.Link(nameof(GetAudits),
                        new
                        {
                            pageSize = auditsResourceParameters.PageSize,
                            pageNumber = auditsResourceParameters.PageNumber + 1,
                            orderBy = auditsResourceParameters.OrderBy
                        });
                default:
                    return Url.Link(nameof(GetAudits),
                        new
                        {
                            pageSize = auditsResourceParameters.PageSize,
                            pageNumber = auditsResourceParameters.PageNumber,
                            orderBy = auditsResourceParameters.OrderBy
                        });
            }
        }
    }
}
