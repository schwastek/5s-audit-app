using Core.OrderByService;
using Features.Audit.List;
using System.Collections.Generic;

namespace Features.Audit.Mappers;

public sealed class AuditOrderByMapper : OrderByMapper<ListAuditsQuery, Domain.Audit>
{
    public AuditOrderByMapper()
    {
        Mappings.Add(
            nameof(Dto.AuditDto.Author),
            new List<MappedOrderByParameter>() {
                new MappedOrderByParameter { PropertyName = nameof(Domain.Audit.Author), Reverse = false }
            }
        );

        Mappings.Add(
            nameof(Dto.AuditDto.Area),
            new List<MappedOrderByParameter>() {
                new MappedOrderByParameter { PropertyName = nameof(Domain.Audit.Area), Reverse = false }
            }
        );
    }
}
