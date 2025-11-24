using Api.Exceptions;
using Api.Requests.AuditActions.Update;
using Data.Context;
using Domain;
using Domain.Constants;
using Domain.Exceptions;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IntegrationTests.AuditActions;

[TestFixture]
internal class UpdateAuditActionTestFixture : BaseTestFixture
{
    private ICollection<Question>? _questions;
    private Audit? _audit;
    private AuditAction? _auditAction;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _questions = TestDataService.BuildQuestions();
        _audit = TestDataService.BuildAudit(_questions);
        _auditAction = TestDataService.BuildAuditAction();
        _audit.AddActions(_auditAction);

        await DbContext.AddRangeAsync(_questions);
        await DbContext.AddAsync(_audit);

        await DbContext.SaveChangesAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        var audit = await GetAudit(_audit?.AuditId);
        if (audit is not null) DbContext.Audits.Remove(audit);
        await DbContext.SaveChangesAsync();

        if (_questions is not null) DbContext.Questions.RemoveRange(_questions);
        await DbContext.SaveChangesAsync();
    }

    [Test]
    public async Task Should_update_audit_action()
    {
        var request = new UpdateAuditActionRequest()
        {
            Description = TestValueGenerator.GenerateString(),
            IsComplete = true,
            LastVersion = _auditAction!.Version
        };
        var now = DateTimeOffset.UtcNow;

        var response = await Client.PutAsJsonAsync($"api/actions/{_auditAction!.AuditActionId}", request);
        response.EnsureSuccessStatusCode();

        var audit = await GetAudit(_audit?.AuditId);
        Assert.That(audit, Is.Not.Null);
        Assert.That(audit.Actions, Is.Not.Empty);
        Assert.That(audit.Actions, Has.Count.EqualTo(1));

        var auditAction = audit!.Actions.First();
        Assert.Multiple(() =>
        {
            Assert.That(auditAction.AuditId, Is.EqualTo(_audit!.AuditId));
            Assert.That(auditAction.AuditActionId, Is.EqualTo(_auditAction.AuditActionId));
            Assert.That(auditAction.Description, Is.EqualTo(request.Description));
            Assert.That(auditAction.IsComplete, Is.EqualTo(request.IsComplete));
        });
        Assert.Multiple(() =>
        {
            Assert.That(auditAction.CreatedBy, Is.EqualTo(_auditAction.CreatedBy));
            Assert.That(auditAction.CreatedAt, Is.EqualTo(_auditAction.CreatedAt));
            Assert.That(auditAction.ModifiedBy, Is.EqualTo(AuditUsers.Test));
            Assert.That(auditAction.ModifiedAt, Is.GreaterThan(now));
            Assert.That(auditAction.Version, Is.GreaterThan(request.LastVersion));
        });
    }

    [Test]
    public async Task Should_throw_error_when_concurrency_conflict()
    {
        var request = new UpdateAuditActionRequest()
        {
            Description = TestValueGenerator.GenerateString(),
            IsComplete = true,
            LastVersion = -1
        };

        var response = await Client.PutAsJsonAsync($"api/actions/{_auditAction!.AuditActionId}", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.That(content, Is.Not.Null);
        Assert.That(content.Title, Is.EqualTo("ConcurrencyConflict"));
    }

    [Test]
    public async Task Should_throw_error_when_missing_required_field()
    {
        var request = new UpdateAuditActionRequest();

        var response = await Client.PutAsJsonAsync($"api/actions/{Guid.Empty}", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();

        Assert.That(content, Is.Not.Null);
        Assert.That(content.Errors, Is.Not.Empty);
        Assert.That(content.Errors, Is.EquivalentTo(new List<string> {
            ErrorCodes.AuditAction.AuditActionIdIsRequired,
            ErrorCodes.AuditAction.AuditActionDescriptionIsRequired
        }));
    }

    [Test]
    public async Task Should_throw_error_when_audit_action_does_not_exist()
    {
        var request = new UpdateAuditActionRequest()
        {
            Description = TestValueGenerator.GenerateString(),
            IsComplete = true
        };

        var response = await Client.PutAsJsonAsync($"api/actions/{Guid.NewGuid()}", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();

        Assert.That(content, Is.Not.Null);
        Assert.That(content.Errors, Is.Not.Empty);
        Assert.That(content.Errors, Is.EquivalentTo(new List<string> { ErrorCodes.AuditAction.AuditActionDoesNotExist }));
    }

    [Test]
    public async Task Should_throw_error_when_description_is_too_long()
    {
        var request = new UpdateAuditActionRequest()
        {
            Description = TestValueGenerator.GenerateString(AuditActionConstants.DescriptionMaxLength + 1),
            IsComplete = true
        };

        var response = await Client.PutAsJsonAsync($"api/actions/{_auditAction!.AuditActionId}", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();


        Assert.That(content, Is.Not.Null);
        Assert.That(content.Errors, Is.Not.Empty);
        Assert.That(content.Errors, Is.EquivalentTo(new List<string> { ErrorCodes.AuditAction.AuditActionDescriptionIsTooLong }));
    }

    private async Task<Audit?> GetAudit(Guid? id)
    {
        if (id is null) return null;

        // Bust DbContext cache - reload entity to avoid conflicts when saving,
        // as the same entity has been modified by another DbContext instance (API).
        DbContext.ChangeTracker.Clear();

        var audit = await DbContext.Audits
            .Where(x => x.AuditId.Equals(id))
            .Include(x => x.Actions)
            .FirstOrDefaultAsync();

        return audit;
    }
}
