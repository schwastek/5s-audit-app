using System;

namespace Api.Models
{
    public class AnswerForCreationDto
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public string AnswerType { get; set; }
        public string AnswerText { get; set; }
    }
}
