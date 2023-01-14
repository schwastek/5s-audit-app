using Api.Domain;
using System.Collections.Generic;
using Xunit;

namespace Api.UnitTests;

public class AuditScoreCalculationTest
{
    [Fact]
    public void Score_ShouldBe50()
    {
        // Arrange
        var answers = new List<Answer>
        {
            new Answer
            {
                AnswerText = "10"
            },
            new Answer
            {
                AnswerText = "0"
            }
        };

        var audit = new Audit
        {
            Answers = answers
        };

        // Act
        var score = audit.CalculateScore();

        // Assert
        Assert.Equal(0.5, score);
    }

    [Fact]
    public void Score_ShouldBe100()
    {
        // Arrange
        var answers = new List<Answer>
        {
            new Answer
            {
                AnswerText = "5"
            },
            new Answer
            {
                AnswerText = "5"
            }
        };

        var audit = new Audit
        {
            Answers = answers
        };

        // Act
        var score = audit.CalculateScore();

        // Assert
        Assert.Equal(1, score);
    }

    [Fact]
    public void Score_ShouldBe0()
    {
        // Arrange
        var answers = new List<Answer>
        {
            new Answer
            {
                AnswerText = "0"
            },
            new Answer
            {
                AnswerText = "0"
            }
        };

        var audit = new Audit
        {
            Answers = answers
        };

        // Act
        var score = audit.CalculateScore();

        // Assert
        Assert.Equal(0, score);
    }
}
