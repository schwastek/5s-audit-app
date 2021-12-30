using System;

namespace Api.Models
{
    public class AnswerDto
    {
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string AnswerType { get; set; }
        public string AnswerText { get; set; }
    }
}
