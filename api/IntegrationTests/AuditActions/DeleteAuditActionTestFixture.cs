using Api.Exceptions;
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

namespace IntegrationTests.AuditActions;

[TestFixture]
internal sealed class DeleteAuditActionTestFixture : BaseTestFixture
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
        _audit.AddActions([_auditAction]);

        await DbContext.AddRangeAsync(_questions);
        await DbContext.AddAsync(_audit);

        await DbContext.SaveChangesAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        // Bust DbContext cache - reload entity to avoid conflicts when saving,
        // as the same entity has been modified by another DbContext instance (API).
        DbContext.ChangeTracker.Clear();
        var audit = await GetAudit(_audit!.AuditId);

        // Remove
        if (audit is not null) DbContext.Audits.Remove(audit);
        if (_questions is not null) DbContext.Questions.RemoveRange(_questions);

        await DbContext.SaveChangesAsync();
    }

    [Test]
    public async Task Should_delete_audit_action()
    {
        var response = await Client.DeleteAsync($"api/actions/{_auditAction!.AuditActionId}");
        response.EnsureSuccessStatusCode();

        var audit = await GetAudit(_audit!.AuditId);
        Assert.That(audit, Is.Not.Null);
        Assert.That(audit.Actions, Is.Empty);
    }

    [Test]
    public async Task Should_throw_error_when_missing_id()
    {
        var response = await Client.DeleteAsync($"api/actions/{Guid.Empty}");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();

        Assert.That(content, Is.Not.Null);
        Assert.That(content.Errors, Is.EquivalentTo([ErrorCodes.AuditAction.ActionIdIsRequired]));
    }

    [Test]
    public async Task Should_throw_error_when_audit_action_does_not_exist()
    {
        var response = await Client.DeleteAsync($"api/actions/{Guid.NewGuid()}");
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        Assert.That(content, Is.Not.Null);
        Assert.That(content.Errors, Is.EquivalentTo([ErrorCodes.AuditAction.DoesNotExist]));
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
