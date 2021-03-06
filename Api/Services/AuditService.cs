using Api.DbContexts;
using Api.Domain;
using Api.Helpers;
using Api.Mappers;
using Api.Models;
using Api.ResourceParameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services
{
    public class AuditService
    {
        private readonly LeanAuditorContext _context;
        private readonly AuditMapper _auditMapper;
        private readonly IPropertyMappingService _propertyMappingService;

        public AuditService(LeanAuditorContext context, AuditMapper auditMapper, IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _auditMapper = auditMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<IEnumerable<Audit>> GetAudits()
        {
            IEnumerable<Audit> audits = await _context.Audits.ToListAsync();

            return audits;
        }

        public async Task<PagedList<Audit>> GetAudits(AuditsUrlQueryParameters auditsResourceParameters)
        {
            IQueryable<Audit> collection = _context.Audits
                .Include(audit => audit.Answers)
                .ThenInclude(answer => answer.Question)
                .AsQueryable();

            bool hasOrderBy = !string.IsNullOrWhiteSpace(auditsResourceParameters.OrderBy);

            if (hasOrderBy)
            {
                Dictionary<string, PropertyMappingValue> auditPropertyMapping =
                    _propertyMappingService.GetPropertyMapping<AuditDto, Audit>();

                collection = collection.ApplySort(auditsResourceParameters.OrderBy,
                    auditPropertyMapping);
            }

            PagedList<Audit> pagedItems = await PagedList<Audit>.CreateAsync(collection,
                auditsResourceParameters.PageNumber,
                auditsResourceParameters.PageSize);

            return pagedItems;
        }

        public bool AuditExists(Guid auditId)
        {
            if (auditId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(auditId));
            }

            return _context.Audits.Any(a => a.AuditId == auditId);
        }
    }
}
