using Api.Contracts.AuditAction.Requests;
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

namespace IntegrationTests.AuditActions;

[TestFixture]
internal class SaveAuditActionTestFixture : BaseTestFixture
{
    private ICollection<Question>? _questions;
    private Audit? _audit;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _questions = TestDataService.BuildQuestions();
        _audit = TestDataService.BuildAudit(_questions);

        await DbContext.AddRangeAsync(_questions);
        await DbContext.AddAsync(_audit);

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
    public async Task Should_save_audit_action()
    {
        var request = new SaveAuditActionRequest()
        {
            AuditId = _audit!.AuditId,
            AuditActionId = Guid.NewGuid(),
            Description = TestValueGenerator.GenerateString(AuditAction.DescriptionMaxLength)
        };

        var response = await Client.PostAsJsonAsync($"api/actions", request);
        response.EnsureSuccessStatusCode();

        var audit = await GetAudit(_audit!.AuditId);

        audit.Should().NotBeNull();
        audit!.Actions.Should().NotBeEmpty();
        audit!.Actions.Should().HaveCount(1);
        var auditAction = audit!.Actions.First();
        auditAction.AuditId.Should().Be(request.AuditId);
        auditAction.AuditActionId.Should().Be(request.AuditActionId);
        auditAction.Description.Should().Be(request.Description);
        auditAction.IsComplete.Should().BeFalse();
    }

    [Test]
    public async Task Should_throw_error_when_audit_does_not_exist()
    {
        var request = new SaveAuditActionRequest()
        {
            AuditId = Guid.NewGuid(),
            AuditActionId = Guid.NewGuid(),
            Description = TestValueGenerator.GenerateString()
        };

        var response = await Client.PostAsJsonAsync($"api/actions", request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();

        content.Should().NotBeNull();
        content!.Errors.Should().BeEquivalentTo([ErrorCodes.Audit.DoesNotExist]);
    }

    [Test]
    public async Task Should_throw_error_when_missing_required_field()
    {
        var request = new SaveAuditActionRequest();

        var response = await Client.PostAsJsonAsync($"api/actions", request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();

        content.Should().NotBeNull();
        content!.Errors.Should().BeEquivalentTo([
            ErrorCodes.AuditAction.ActionIdIsRequired,
            ErrorCodes.Audit.AuditIdIsRequired,
            ErrorCodes.AuditAction.DescriptionIsRequired
        ]);
    }

    [Test]
    public async Task Should_throw_error_when_description_is_too_long()
    {
        var request = new SaveAuditActionRequest()
        {
            AuditId = _audit!.AuditId,
            AuditActionId = Guid.NewGuid(),
            Description = TestValueGenerator.GenerateString(AuditAction.DescriptionMaxLength + 1)
        };

        var response = await Client.PostAsJsonAsync($"api/actions", request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();

        content.Should().NotBeNull();
        content!.Errors.Should().BeEquivalentTo([ErrorCodes.AuditAction.DescriptionIsTooLong]);
    }

    private async Task<Audit?> GetAudit(Guid id)
    {
        var audit = await DbContext.Audits
            .AsNoTracking()
            .Where(x => x.AuditId.Equals(id))
            .Include(x => x.Actions)
            .FirstOrDefaultAsync();

        return audit;
    }
}
