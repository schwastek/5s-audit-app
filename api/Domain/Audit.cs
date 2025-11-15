using Domain.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain;

public sealed class Audit : IAuditableEntity
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
    private readonly List<Answer> _answers = [];
    public IReadOnlyCollection<Answer> Answers => _answers.AsReadOnly();

    private readonly List<AuditAction> _actions = [];
    public IReadOnlyCollection<AuditAction> Actions => _actions.AsReadOnly();

    public string CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string ModifiedBy { get; private set; }
    public DateTimeOffset ModifiedAt { get; private set; }

    // EF Core supports using parameterized constructors to map entity properties 
    // during materialization (when retrieving data from the database and creating entity instances).
    // Adding a parameterless constructor would be enough for Entity Framework,
    // but using parameters resolves nullable reference warnings.
    // See: https://learn.microsoft.com/en-us/ef/core/modeling/constructors
    private Audit(Guid auditId, string author, string area, DateTime startDate, DateTime endDate,
        string createdBy, DateTimeOffset createdAt, string modifiedBy, DateTimeOffset modifiedAt)
    {
        AuditId = auditId;
        Area = area;
        Author = author;
        StartDate = startDate;
        EndDate = endDate;

        // Set auditable properties.
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        ModifiedAt = modifiedAt;
        ModifiedBy = modifiedBy;
    }

    public static Audit Create(Guid auditId, string author, string area, DateTime startDate, DateTime endDate)
    {
        // Set auditable properties.
        var createdBy = AuditPlaceholders.Unknown;
        var createdAt = DateTimeOffset.UtcNow;

        var audit = new Audit(
            auditId: auditId,
            author: author,
            area: area,
            startDate: startDate,
            endDate: endDate,
            createdBy: createdBy,
            createdAt: createdAt,
            modifiedBy: createdBy,
            modifiedAt: createdAt);

        return audit;
    }

    public void AddAnswers(params Answer[] answers)
    {
        _answers.AddRange(answers);
    }

    public void AddAnswers(IEnumerable<Answer> answers)
    {
        _answers.AddRange(answers);
    }

    public void AddActions(params AuditAction[] auditActions)
    {
        _actions.AddRange(auditActions);
    }

    public void AddActions(IEnumerable<AuditAction> auditActions)
    {
        _actions.AddRange(auditActions);
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
