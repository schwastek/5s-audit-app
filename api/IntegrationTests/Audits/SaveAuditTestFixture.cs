using Api.Exceptions;
using Api.Requests.Answers.Dto;
using Api.Requests.AuditActions.Dto;
using Api.Requests.Audits.Save;
using Data.Context;
using Domain;
using Domain.Exceptions;
using IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IntegrationTests.Audits;

[TestFixture]
internal sealed class SaveAuditTestFixture : BaseTestFixture
{
    private ICollection<Question>? _questions;
    private Audit? _audit;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _questions = TestDataService.BuildQuestions(1);

        await DbContext.AddRangeAsync(_questions);
        await DbContext.SaveChangesAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (_audit is not null) DbContext.Audits.Remove(_audit);
        if (_questions is not null) DbContext.Questions.RemoveRange(_questions);

        await DbContext.SaveChangesAsync();
    }

    [Test]
    public async Task Should_save_audit()
    {
        // Arrange
        var answers = new List<AnswerForCreationDto>(1)
        {
            new()
            {
                QuestionId = _questions!.First().QuestionId,
                AnswerId = Guid.NewGuid(),
                AnswerType = "number",
                AnswerText = "5"
            }
        };
        var actions = new List<AuditActionForCreationDto>(1)
        {
            new()
            {
                AuditActionId = Guid.NewGuid(),
                Description = TestValueGenerator.GenerateString()
            }
        };
        var request = new SaveAuditRequest()
        {
            AuditId = Guid.NewGuid(),
            Author = TestValueGenerator.GenerateString(),
            Area = TestValueGenerator.GenerateString(),
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMinutes(15),
            Answers = answers,
            Actions = actions
        };
        var now = DateTimeOffset.UtcNow;

        // Act
        var response = await Client.PostAsJsonAsync($"api/audits", request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<SaveAuditResponse>();

        // Assert
        Assert.That(content, Is.Not.Null);
        Assert.That(content.AuditId, Is.EqualTo(request.AuditId));

        _audit = await GetAudit(request.AuditId);

        Assert.That(_audit, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(_audit.Author, Is.EqualTo(request.Author));
            Assert.That(_audit.Area, Is.EqualTo(request.Area));
            Assert.That(_audit.StartDate, Is.EqualTo(request.StartDate));
            Assert.That(_audit.EndDate, Is.EqualTo(request.EndDate));
            Assert.That(_audit.Answers, Has.Count.EqualTo(request.Answers.Count));
            Assert.That(_audit.Actions, Has.Count.EqualTo(request.Actions.Count));
        });
        Assert.Multiple(() =>
        {
            Assert.That(_audit.CreatedBy, Is.EqualTo(AuditUsers.Test));
            Assert.That(_audit.CreatedAt, Is.GreaterThan(now));
            Assert.That(_audit.ModifiedBy, Is.EqualTo(_audit.CreatedBy));
            Assert.That(_audit.ModifiedAt, Is.EqualTo(_audit.CreatedAt));
        });

        // Map to common type - use anonymous types for comparison.
        var actualAnswers = _audit.Answers.Select(x => new
        {
            QuestionId = x.QuestionId,
            AnswerId = x.AnswerId,
            AnswerType = x.AnswerType,
            AnswerText = x.AnswerText
        });
        var expectedAnswers = request.Answers.Select(x => new
        {
            QuestionId = x.QuestionId,
            AnswerId = x.AnswerId,
            AnswerType = x.AnswerType,
            AnswerText = x.AnswerText
        });
        Assert.That(actualAnswers, Is.EquivalentTo(expectedAnswers));

        var actualActions = _audit.Actions.Select(x => new
        {
            AuditActionId = x.AuditActionId,
            Description = x.Description
        });
        var expectedActions = request.Actions.Select(x => new
        {
            AuditActionId = x.AuditActionId,
            Description = x.Description
        });
        Assert.That(actualActions, Is.EquivalentTo(expectedActions));
    }

    [Test]
    public async Task Should_throw_error_when_missing_required_field()
    {
        // Arrange
        var request = new SaveAuditRequest()
        {
            Answers = []
        };

        // Act
        var response = await Client.PostAsJsonAsync($"api/audits", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();

        // Assert
        Assert.That(content, Is.Not.Null);
        Assert.That(content.Errors, Is.Not.Empty);
        Assert.That(content.Errors, Is.EquivalentTo(new List<string> {
            ErrorCodes.Audit.AuditIdIsRequired,
            ErrorCodes.Audit.AuditAuthorIsRequired,
            ErrorCodes.Audit.AuditAreaIsRequired,
            ErrorCodes.Audit.AuditStartDateIsRequired,
            ErrorCodes.Audit.AuditEndDateIsRequired,
            ErrorCodes.Audit.AuditAnswersIsRequired
        }));
    }

    private async Task<Audit?> GetAudit(Guid id)
    {
        var audit = await DbContext.Audits
            .AsNoTracking()
            .Where(x => x.AuditId.Equals(id))
            .Include(x => x.Answers)
            .Include(x => x.Actions)
            .FirstOrDefaultAsync();

        return audit;
    }
}
