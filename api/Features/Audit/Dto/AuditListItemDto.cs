using Core.MappingService;
using System;

namespace Features.Audit.Dto;

public class AuditListItemDto
{
    public required Guid AuditId { get; set; }
    public required string Author { get; set; }
    public required string Area { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required double Score { get; set; }
}

public class AuditListItemDtoMapper :
    IMapper<Domain.Audit, AuditListItemDto>,
    IMapper<AuditListItemDto, Api.Contracts.Audit.Dto.AuditListItemDto>
{
    public AuditListItemDto Map(Domain.Audit src)
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

    public Api.Contracts.Audit.Dto.AuditListItemDto Map(AuditListItemDto src)
    {
        return new Api.Contracts.Audit.Dto.AuditListItemDto()
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
