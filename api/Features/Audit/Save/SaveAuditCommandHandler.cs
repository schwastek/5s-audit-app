using Data.DbContext;
using Features.Audit.Dto;
using Api.Mappers.MappingService;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Audit.Save;

public sealed class SaveAuditCommandHandler : IRequestHandler<SaveAuditCommand, SaveAuditCommandResult>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;

    public SaveAuditCommandHandler(LeanAuditorContext context, IMappingService mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<SaveAuditCommandResult> Handle(SaveAuditCommand command, CancellationToken cancellationToken)
    {
        var audit = new Domain.Audit()
        {
            AuditId = command.AuditId,
            Author = command.Author,
            Area = command.Area,
            StartDate = command.StartDate,
            EndDate = command.EndDate
        };

        audit.CalculateScore();

        context.Audits.Add(audit);
        context.SaveChanges();

        AuditDto auditDto = mapper.Map<Domain.Audit, AuditDto>(audit);

        return await Task.FromResult(new SaveAuditCommandResult() { Audit = auditDto });
    }
}
