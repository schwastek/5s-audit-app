using Api.Domain;
using Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace Api.Mappers
{
    public class AuditMapper
    {
        private readonly AnswerMapper _answerMapper;
        private readonly AuditActionMapper _auditActionMapper;

        public AuditMapper(AnswerMapper answerMapper, AuditActionMapper auditActionMapper)
        {
            _answerMapper = answerMapper;
            _auditActionMapper = auditActionMapper;
        }

        public Audit Map(AuditForCreationDto request)
        {
            ICollection<Answer> answers = request.Answers
                .Select(answer => _answerMapper.Map(answer))
                .ToList();

            ICollection<AuditAction> actions = request.Actions
                .Select(a => _auditActionMapper.Map(a))
                .ToList();

            Audit audit = new()
            {
                AuditId = request.AuditId,
                Author = request.Author,
                Area = request.Area,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Answers = answers,
                Actions = actions
            };

            return audit;
        }

        public AuditDto Map(Audit audit)
        {
            ICollection<AnswerDto> answers = audit.Answers
                .Select(answer => _answerMapper.Map(answer))
                .ToList();

            ICollection<AuditActionDto> actions = audit.Actions
                .Select(a => _auditActionMapper.Map(a))
                .ToList();

            AuditDto auditDto = new()
            {
                AuditId = audit.AuditId,
                Area = audit.Area,
                Author = audit.Author,
                StartDate = audit.StartDate,
                EndDate = audit.EndDate,
                Score = audit.CalculateScore(),
                Answers = answers,
                Actions = actions
            };

            return auditDto;
        }
    }
}
