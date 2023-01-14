using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Api.Domain;

public class Audit
{
    [Required]
    public Guid AuditId { get; set; }
    [Required]
    public string Author { get; set; }
    [Required]
    public string Area { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public ICollection<AuditAction> Actions { get; set; } = new List<AuditAction>();

    public double CalculateScore()
    {
        // Convert string answers to numbers: "4" to 4
        List<int> parsedAnswers = Answers
                .Select(a => ParseStringToNumber(a.AnswerText))
                .ToList();

        double score = Calculate(parsedAnswers);

        return score;
    }

    private static int ParseStringToNumber(string input)
    {
        bool isParsable = Int32.TryParse(input, out int number);

        if (!isParsable)
        {
            return 0;
        }

        return number;
    }

    private static double Calculate(IEnumerable<int> answers)
    {
        bool isEmpty = !answers.Any();
        if (isEmpty) return 0;

        int numberOfAnswers = answers.Count();
        int totalPointsScored = answers.Sum();
        int maxPointsForAnswer = answers.Max();
        int totalPointsPossible = numberOfAnswers * maxPointsForAnswer;

        // Prevent division by zero
        if (totalPointsPossible == 0) return 0;

        double score = (double)totalPointsScored / (double)totalPointsPossible;

        return score;
    }
}
