using Infrastructure.MappingService;

namespace Api.Requests.Audits.Dto;

public sealed class AuditListItemDtoMapper : IMapper<Features.Audits.Dto.AuditListItemDto, Requests.Audits.Dto.AuditListItemDto>
{
    public AuditListItemDto Map(Features.Audits.Dto.AuditListItemDto src)
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
