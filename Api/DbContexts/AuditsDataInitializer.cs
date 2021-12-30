using Api.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Api.DbContexts
{
    public class AuditsDataInitializer
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            Guid[] auditUuidPool =
            {
                Guid.Parse("f4940d26-7c0a-4ab6-b1cd-da8f708c5819"),
                Guid.Parse("a065c86d-3846-41bf-a268-423c743ca064")
            };

            Guid[] questionUuidPool =
            {
                Guid.Parse("af70e9f3-9a70-4178-80dc-87d38bb1c810"),
                Guid.Parse("cb1a8298-9eca-4684-8c7c-1ed24298cca2"),
                Guid.Parse("6efa4fdb-b626-4af0-aea8-0a38399502d0"),
                Guid.Parse("bfab9a5c-4e74-4eff-a6ed-7b39748378ad"),
                Guid.Parse("eb2ef0b3-6af5-4d53-89d8-7b069ccb343c")
            };

            Guid[,] answerUuidPool = new Guid[,]
            {
                // Answer IDs for audit #1
                {
                    Guid.Parse("76551e14-0791-4d3b-99fd-387e25b9c96b"),
                    Guid.Parse("0c24c0d3-2092-4266-ba7c-9e36c7b3256d"),
                    Guid.Parse("dd3ae4e3-9651-4142-9a4d-63e33cad11cb"),
                    Guid.Parse("30deb494-6e45-4eb5-ab99-cc2f8eac981b"),
                    Guid.Parse("690da001-2594-4f64-a7d5-68b1fa095493"),
                },

                // Answer IDs for audit #2
                {
                    Guid.Parse("fcaf6a07-c661-4977-bc81-ce4f0675b344"),
                    Guid.Parse("1939baab-b109-42b6-be49-7eee38575f69"),
                    Guid.Parse("833bd0a9-2e2c-4a4d-ab7e-59597324f191"),
                    Guid.Parse("4d539fc1-bad0-4380-90a4-1ed9d305150b"),
                    Guid.Parse("cc0e98ee-090d-4046-af4c-7bd1e05994b9")
                }
            };

            modelBuilder.Entity<Audit>().HasData(
                new Audit
                {
                    AuditId = auditUuidPool[0],
                    Author = "John",
                    Area = "warehouse",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMinutes(15)
                },
                new Audit
                {
                    AuditId = auditUuidPool[1],
                    Author = "John",
                    Area = "assembly",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMinutes(12)
                }
                );

            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    QuestionId = questionUuidPool[0],
                    QuestionText = "Are all tools in the work area currently in use?"
                },
                new Question
                {
                    QuestionId = questionUuidPool[1],
                    QuestionText = "Are all tools or parts off the floor?"
                },
                new Question
                {
                    QuestionId = questionUuidPool[2],
                    QuestionText = "Are all posted work instructions, notes and drawing currently in use?"
                },
                new Question
                {
                    QuestionId = questionUuidPool[3],
                    QuestionText = "Are workstations and walkways clear of unnecessary items and clutter?"
                },
                new Question
                {
                    QuestionId = questionUuidPool[4],
                    QuestionText = "Are occasionally used items stored separately?"
                });

            // Answers for audit #1
            modelBuilder.Entity<Answer>().HasData(
                new Answer
                {
                    AuditId = auditUuidPool[0],
                    QuestionId = questionUuidPool[0],
                    AnswerId = answerUuidPool[0, 0],
                    AnswerText = "1",
                    AnswerType = "number"
                },
                new Answer
                {
                    AuditId = auditUuidPool[0],
                    QuestionId = questionUuidPool[1],
                    AnswerId = answerUuidPool[0, 1],
                    AnswerText = "2",
                    AnswerType = "number"
                },
                new Answer
                {
                    AuditId = auditUuidPool[0],
                    QuestionId = questionUuidPool[2],
                    AnswerId = answerUuidPool[0, 2],
                    AnswerText = "3",
                    AnswerType = "number"
                },
                new Answer
                {
                    AuditId = auditUuidPool[0],
                    QuestionId = questionUuidPool[3],
                    AnswerId = answerUuidPool[0, 3],
                    AnswerText = "4",
                    AnswerType = "number"
                },
                new Answer
                {
                    AuditId = auditUuidPool[0],
                    QuestionId = questionUuidPool[4],
                    AnswerId = answerUuidPool[0, 4],
                    AnswerText = "5",
                    AnswerType = "number"
                });

            // Answers for audit #2
            modelBuilder.Entity<Answer>().HasData(
                new Answer
                {
                    AuditId = auditUuidPool[1],
                    QuestionId = questionUuidPool[0],
                    AnswerId = answerUuidPool[1, 0],
                    AnswerText = "1",
                    AnswerType = "number"
                },
                new Answer
                {
                    AuditId = auditUuidPool[1],
                    QuestionId = questionUuidPool[1],
                    AnswerId = answerUuidPool[1, 1],
                    AnswerText = "1",
                    AnswerType = "number"
                },
                new Answer
                {
                    AuditId = auditUuidPool[1],
                    QuestionId = questionUuidPool[2],
                    AnswerId = answerUuidPool[1, 2],
                    AnswerText = "1",
                    AnswerType = "number"
                },
                new Answer
                {
                    AuditId = auditUuidPool[1],
                    QuestionId = questionUuidPool[3],
                    AnswerId = answerUuidPool[1, 3],
                    AnswerText = "1",
                    AnswerType = "number"
                },
                new Answer
                {
                    AuditId = auditUuidPool[1],
                    QuestionId = questionUuidPool[4],
                    AnswerId = answerUuidPool[1, 4],
                    AnswerText = "1",
                    AnswerType = "number"
                });
        }
    }
}
