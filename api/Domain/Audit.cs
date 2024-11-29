using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain;

public sealed class Audit
{
    public Guid AuditId { get; private set; }
    public string Author { get; private set; }
    public string Area { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public double Score { get; private set; }

    // Collection navigations, which contain references to multiple related entities, should always be non-nullable.
    // An empty collection means that no related entities exist, but the list itself should never be null.
    // See: https://learn.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types#required-navigation-properties
    private readonly List<Answer> answers = [];
    public IReadOnlyCollection<Answer> Answers => answers.AsReadOnly();

    private readonly List<AuditAction> actions = [];
    public IReadOnlyCollection<AuditAction> Actions => actions;

    // EF Core supports using parameterized constructors to map entity properties 
    // during materialization (when retrieving data from the database and creating entity instances).
    // Adding a parameterless constructor would be enough for Entity Framework,
    // but using parameters resolves nullable reference warnings.
    // See: https://learn.microsoft.com/en-us/ef/core/modeling/constructors
    private Audit(Guid auditId, string author, string area, DateTime startDate, DateTime endDate)
    {
        AuditId = auditId;
        Area = area;
        Author = author;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static Audit Create(Guid auditId, string author, string area, DateTime startDate, DateTime endDate)
    {
        var audit = new Audit(auditId, author, area, startDate, endDate);

        return audit;
    }

    public void AddAnswers(params Answer[] answers)
    {
        this.answers.AddRange(answers);
    }

    public void AddAnswers(IEnumerable<Answer> answers)
    {
        this.answers.AddRange(answers);
    }

    public void AddActions(params AuditAction[] auditActions)
    {
        this.actions.AddRange(auditActions);
    }

    public void AddActions(IEnumerable<AuditAction> auditActions)
    {
        this.actions.AddRange(auditActions);
    }

    public void CalculateScore()
    {
        // Convert string answers to numbers: "4" to 4
        List<int> parsedAnswers = Answers
                .Select(a => ParseStringToNumber(a.AnswerText))
                .ToList();

        double score = Calculate(parsedAnswers);

        Score = score;
    }

    // TODO: Create extension method.
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
