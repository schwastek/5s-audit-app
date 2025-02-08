using Api.Exceptions;
using Api.Requests.Audits.Get;
using Domain;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IntegrationTests.Audits;

[TestFixture]
internal sealed class GetAuditTestFixture : BaseTestFixture
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
    public async Task Should_return_audit_by_id()
    {
        var request = new GetAuditRequest()
        {
            Id = _audit!.AuditId
        };
        var response = await Client.GetAsync($"api/audits/{request.Id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<GetAuditResponse>();

        Assert.That(content, Is.Not.Null);
        Assert.That(content.AuditId, Is.EqualTo(request.Id));
        Assert.That(content.Author, Is.EqualTo(_audit!.Author));
        Assert.That(content.Area, Is.EqualTo(_audit!.Area));
        Assert.That(content.StartDate, Is.EqualTo(_audit!.StartDate));
        Assert.That(content.EndDate, Is.EqualTo(_audit!.EndDate));
    }

    [Test]
    public async Task Should_throw_error_when_audit_does_not_exist()
    {
        var request = new GetAuditRequest()
        {
            Id = Guid.NewGuid()
        };
        var response = await Client.GetAsync($"api/audits/{request.Id}");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();

        Assert.That(content, Is.Not.Null);
        Assert.That(content.Errors, Is.Not.Empty);
        Assert.That(content.Errors, Does.Contain(ErrorCodes.Audit.DoesNotExist));
    }
}
