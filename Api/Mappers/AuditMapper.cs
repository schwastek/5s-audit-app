using Api.Domain;
using Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace Api.Mappers
{
    public class AuditMapper
    {
        private readonly AnswerMapper _answerMapper;

        public AuditMapper(AnswerMapper answerMapper)
        {
            _answerMapper = answerMapper;
        }

        public Audit Map(AuditForCreationDto request)
        {
            ICollection<Answer> answers = request.Answers
                .Select(answer => _answerMapper.Map(answer))
                .ToList();

            Audit audit = new()
            {
                AuditId = request.AuditId,
                Author = request.Author,
                Area = request.Area,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Answers = answers
            };

            return audit;
        }

        public AuditDto Map(Audit audit)
        {
            ICollection<AnswerDto> answers = audit.Answers
                .Select(answer => _answerMapper.Map(answer))
                .ToList();

            AuditDto auditDto = new()
            {
                AuditId = audit.AuditId,
                Area = audit.Area,
                Author = audit.Author,
                StartDate = audit.StartDate,
                EndDate = audit.EndDate,
                Score = audit.CalculateScore(),
                Answers = answers
            };

            return auditDto;
        }
    }
}
