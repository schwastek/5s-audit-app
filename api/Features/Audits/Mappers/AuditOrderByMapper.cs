using Domain;
using Features.Audits.Dto;
using Features.Audits.List;
using Features.Core.OrderByService;

namespace Features.Audits.Mappers;

public sealed class AuditOrderByMapper : OrderByMapper<ListAuditsQuery, Audit>
{
    public AuditOrderByMapper()
    {
        Mappings.Add(
            nameof(AuditDto.Author),
            [
                new() { PropertyName = nameof(Audit.Author), Reverse = false }
            ]
        );

        Mappings.Add(
            nameof(AuditDto.Area),
            [
                new() { PropertyName = nameof(Audit.Area), Reverse = false }
            ]
        );
    }
}
