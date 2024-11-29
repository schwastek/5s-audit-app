using Domain;
using System;
using Xunit;

namespace UnitTests;

public class AuditScoreCalculationTest
{
    [Fact]
    public void Score_ShouldBe50()
    {
        // Arrange
        var audit = Audit.Create(
            auditId: Guid.NewGuid(),
            author: "John",
            area: "warehouse",
            startDate: DateTime.Parse("2020-01-25T20:06:38.946Z"),
            endDate: DateTime.Parse("2020-01-25T20:36:38.000Z")
        );

        audit.AddAnswers([
             Answer.Create(
                answerId: Guid.NewGuid(),
                questionId: Guid.NewGuid(),
                answerText: "10",
                answerType: "number"
            ),
            Answer.Create(
                answerId: Guid.NewGuid(),
                questionId: Guid.NewGuid(),
                answerText: "0",
                answerType: "number"
            )
        ]);

        // Act
        audit.CalculateScore();

        // Assert
        Assert.Equal(0.5, audit.Score);
    }

    [Fact]
    public void Score_ShouldBe100()
    {
        // Arrange
        var audit = Audit.Create(
            auditId: Guid.NewGuid(),
            author: "John",
            area: "warehouse",
            startDate: DateTime.Parse("2020-01-25T20:06:38.946Z"),
            endDate: DateTime.Parse("2020-01-25T20:36:38.000Z")
        );

        audit.AddAnswers([
             Answer.Create(
                answerId: Guid.NewGuid(),
                questionId: Guid.NewGuid(),
                answerText: "5",
                answerType: "number"
            ),
            Answer.Create(
                answerId: Guid.NewGuid(),
                questionId: Guid.NewGuid(),
                answerText: "5",
                answerType: "number"
            )
        ]);

        // Act
        audit.CalculateScore();

        // Assert
        Assert.Equal(1, audit.Score);
    }

    [Fact]
    public void Score_ShouldBe0()
    {
        // Arrange
        var audit = Audit.Create(
            auditId: Guid.NewGuid(),
            author: "John",
            area: "warehouse",
            startDate: DateTime.Parse("2020-01-25T20:06:38.946Z"),
            endDate: DateTime.Parse("2020-01-25T20:36:38.000Z")
        );

        audit.AddAnswers([
             Answer.Create(
                answerId: Guid.NewGuid(),
                questionId: Guid.NewGuid(),
                answerText: "0",
                answerType: "number"
            ),
            Answer.Create(
                answerId: Guid.NewGuid(),
                questionId: Guid.NewGuid(),
                answerText: "0",
                answerType: "number"
            )
        ]);

        // Act
        audit.CalculateScore();

        // Assert
        Assert.Equal(0, audit.Score);
    }
}
