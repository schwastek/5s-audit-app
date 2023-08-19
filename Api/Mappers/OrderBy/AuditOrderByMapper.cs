using Api.Core.Domain;
using Api.Models;
using System.Collections.Generic;

namespace Api.Mappers.OrderBy;

public sealed class AuditOrderByMapper : OrderByMapper<AuditListDto, Audit>
{
    public AuditOrderByMapper()
    {
        Mappings.Add(
            nameof(AuditListDto.Author),
            new List<MappedOrderByParameter>() {
                new MappedOrderByParameter { PropertyName = nameof(Audit.Author), Reverse = false }
            }
        );

        Mappings.Add(
            nameof(AuditListDto.Area),
            new List<MappedOrderByParameter>() {
                new MappedOrderByParameter { PropertyName = nameof(Audit.Area), Reverse = false }
            }
        );
    }
}