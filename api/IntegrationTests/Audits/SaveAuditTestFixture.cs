using Api.Contracts.Answer.Dto;
using Api.Contracts.Audit.Requests;
using Api.Contracts.AuditAction.Dto;
using Api.Exceptions;
using Domain;
using FluentAssertions;
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

        // Act
        var response = await Client.PostAsJsonAsync($"api/audits", request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<SaveAuditResponse>();

        // Assert
        content.Should().NotBeNull();
        content!.AuditId.Should().Be(request.AuditId);

        _audit = await DbContext.Audits
            .Where(x => x.AuditId.Equals(request.AuditId))
            .AsNoTracking()
            .Include(x => x.Answers)
            .Include(x => x.Actions)
            .FirstOrDefaultAsync();

        _audit.Should().NotBeNull();
        _audit!.Author.Should().Be(request.Author);
        _audit!.Area.Should().Be(request.Area);
        _audit!.StartDate.Should().Be(request.StartDate);
        _audit!.EndDate.Should().Be(request.EndDate);
        _audit!.Answers.Should().HaveCount(request.Answers.Count());
        _audit!.Actions.Should().HaveCount(request.Actions.Count());
        _audit!.Answers.Should().BeEquivalentTo(answers);

        // Map to domain model
        var expectedActions = request.Actions.Select(x => new
        {
            AuditActionId = x.AuditActionId,
            Description = x.Description
        });
        _audit!.Actions.Should().BeEquivalentTo(expectedActions);
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
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();

        // Assert
        var expectedErrors = new[] {
            ErrorCodes.Audit.AuditIdIsRequired,
            ErrorCodes.Audit.AuthorIsRequired,
            ErrorCodes.Audit.AreaIsRequired,
            ErrorCodes.Audit.StartDateIsRequired,
            ErrorCodes.Audit.EndDateIsRequired,
            ErrorCodes.Audit.AnswersIsRequired
        };

        content.Should().NotBeNull();
        content!.Errors.Should().NotBeEmpty();
        content!.Errors.Should().BeEquivalentTo(expectedErrors);
    }
}
