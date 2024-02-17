using Core.MappingService;
using System;

namespace Features.Audit.Dto;

public class AuditListItemDto
{
    public Guid AuditId { get; set; }
    public string Author { get; set; } = null!;
    public string Area { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double Score { get; set; }
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
