using Api.Contracts.Audit.Requests;
using Api.Exceptions;
using Domain;
using FluentAssertions;
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

        content.Should().NotBeNull();
        content!.Audit.AuditId.Should().Be(_audit!.AuditId);
        content!.Audit.Author.Should().Be(_audit!.Author);
        content!.Audit.Area.Should().Be(_audit!.Area);
        content!.Audit.StartDate.Should().Be(_audit!.StartDate);
        content!.Audit.EndDate.Should().Be(_audit!.EndDate);
    }

    [Test]
    public async Task Should_throw_error_when_audit_does_not_exist()
    {
        var request = new GetAuditRequest()
        {
            Id = Guid.NewGuid()
        };
        var response = await Client.GetAsync($"api/audits/{request.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<CustomValidationProblemDetails>();

        content.Should().NotBeNull();
        content!.Errors.Should().NotBeEmpty();
        content!.Errors.Should().Contain(ErrorCodes.Audit.DoesNotExist);
    }
}
