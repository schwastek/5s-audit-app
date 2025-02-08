using Core.MappingService;

namespace Api.Requests.Audits.Dto;

public sealed class AuditListItemDtoMapper : IMapper<Features.Audit.Dto.AuditListItemDto, AuditListItemDto>
{
    public AuditListItemDto Map(Features.Audit.Dto.AuditListItemDto src)
    {
        return new AuditListItemDto()
        {
            AuditId = src.AuditId,
            Author = src.Author,
            Area = src.Area,
            StartDate = src.StartDate,
            EndDate = src.EndDate,
            Score = src.Score
        };
    }
}
