using Api.Exceptions;
using Api.Requests.AuditActions.Save;
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
        Assert.That(audit, Is.Not.Null);
        Assert.That(audit.Actions, Is.Not.Empty);
        Assert.That(audit.Actions, Has.Count.EqualTo(1));

        var auditAction = audit!.Actions.First();
        Assert.Multiple(() =>
        {
            Assert.That(auditAction.AuditId, Is.EqualTo(request.AuditId));
            Assert.That(auditAction.AuditActionId, Is.EqualTo(request.AuditActionId));
            Assert.That(auditAction.Description, Is.EqualTo(request.Description));
            Assert.That(auditAction.IsComplete, Is.False);
        });
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
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();

        Assert.That(content, Is.Not.Null);
        Assert.That(content.Errors, Is.EquivalentTo([ErrorCodes.Audit.DoesNotExist]));
    }

    [Test]
    public async Task Should_throw_error_when_missing_required_field()
    {
        var request = new SaveAuditActionRequest();

        var response = await Client.PostAsJsonAsync($"api/actions", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();

        Assert.That(content, Is.Not.Null);
        Assert.That(content.Errors, Is.EquivalentTo([
            ErrorCodes.AuditAction.ActionIdIsRequired,
            ErrorCodes.Audit.AuditIdIsRequired,
            ErrorCodes.AuditAction.DescriptionIsRequired
        ]));
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
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();

        Assert.That(content, Is.Not.Null);
        Assert.That(content.Errors, Is.EquivalentTo([ErrorCodes.AuditAction.DescriptionIsTooLong]));
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
