using Features.Audits.Dto;
using Features.Audits.List;
using Features.Core.OrderByService;
using Domain;
using System.Collections.Generic;

namespace Features.Audits.Mappers;

public sealed class AuditOrderByMapper : OrderByMapper<ListAuditsQuery, Audit>
{
    public AuditOrderByMapper()
    {
        Mappings.Add(
            nameof(AuditDto.Author),
            new List<MappedOrderByParameter>() {
                new MappedOrderByParameter { PropertyName = nameof(Audit.Author), Reverse = false }
            }
        );

        Mappings.Add(
            nameof(AuditDto.Area),
            new List<MappedOrderByParameter>() {
                new MappedOrderByParameter { PropertyName = nameof(Audit.Area), Reverse = false }
            }
        );
    }
}
