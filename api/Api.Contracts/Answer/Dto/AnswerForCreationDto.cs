using System;

namespace Api.Contracts.Answer.Dto
{
    public class AnswerForCreationDto
    {
        /// <example>af70e9f3-9a70-4178-80dc-87d38bb1c810</example>
        public Guid? QuestionId { get; set; }

        /// <example>1c8d0cd6-49d8-40f1-9324-d471a4463648</example>
        public Guid? AnswerId { get; set; }

        /// <example>number</example>
        public string? AnswerType { get; set; }

        /// <example>4</example>
        public string? AnswerText { get; set; }
    }
}
