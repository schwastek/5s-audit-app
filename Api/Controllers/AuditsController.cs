using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.DbContexts;
using Api.Models;
using Api.Domain;
using Api.Mappers;
using Api.Services;
using Api.ResourceParameters;
using Api.Helpers;
using Api.Extensions;
using Microsoft.AspNetCore.Routing;
using Api.Exceptions;

namespace Api.Controllers
{
    [Route("api/audits")]
    [ApiController]
    public class AuditsController : ControllerBase
    {
        private readonly LeanAuditorContext _context;
        private readonly AuditService _auditService;
        private readonly IMappingService _mapper;
        private readonly IPropertyMappingService _propertyMappingService;

        public AuditsController(LeanAuditorContext context, IMappingService mapper,
            AuditService auditService, IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _mapper = mapper;
            _auditService = auditService;
            _propertyMappingService = propertyMappingService;
        }

        // GET: api/audits
        [HttpGet(Name = nameof(GetAudits))]
        public async Task<IActionResult> GetAudits([FromQuery] AuditsUrlQueryParameters queryParameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<AuditDto, Audit>
                (queryParameters.OrderBy))
            {
                return BadRequest();
            }

            var pagedResult = await _auditService.GetAudits(queryParameters);

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
        public async Task<ActionResult<AuditDto>> GetAudit(Guid id)
        {
            Audit entity = await _context.Audits
                .Include(audit => audit.Actions)
                .Include(audit => audit.Answers)
                .ThenInclude(answer => answer.Question)
                .AsNoTracking()
                .SingleOrDefaultAsync(audit => audit.AuditId.Equals(id));

            if (entity == null)
            {
                throw new AuditNotFoundException(id);
            }

            AuditDto response = _mapper.Map<Audit, AuditDto>(entity);

            return Ok(response);
        }

        // POST: api/Audits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Audit>> PostAudit(AuditForCreationDto request)
        {
            Audit entity = _mapper.Map<AuditForCreationDto, Audit>(request);

            _context.Audits.Add(entity);
            await _context.SaveChangesAsync();

            // Adds a Location header to the response.
            // The Location header specifies the URI of the newly created item.
            return CreatedAtAction("GetAudit", new { id = entity.AuditId }, entity);
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
