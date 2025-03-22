using Api.Requests.Audits.List;
using Domain;
using Features.Core.OrderByService;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IntegrationTests.Audits;

[TestFixture]
internal sealed class ListAuditsTestFixture : BaseTestFixture
{
    private ICollection<Question>? _questions;
    private Audit? _audit1;
    private Audit? _audit2;
    private Audit? _audit3;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _questions = TestDataService.BuildQuestions();
        _audit1 = TestDataService.BuildAudit(_questions, author: "A", area: "Z");
        _audit2 = TestDataService.BuildAudit(_questions, author: "B", area: "Y");
        _audit3 = TestDataService.BuildAudit(_questions, author: "C", area: "X");

        await DbContext.AddRangeAsync(_questions);
        await DbContext.AddAsync(_audit1);
        await DbContext.AddAsync(_audit2);
        await DbContext.AddAsync(_audit3);

        await DbContext.SaveChangesAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (_audit1 is not null) DbContext.Audits.Remove(_audit1);
        if (_audit2 is not null) DbContext.Audits.Remove(_audit2);
        if (_audit3 is not null) DbContext.Audits.Remove(_audit3);
        if (_questions is not null) DbContext.Questions.RemoveRange(_questions);

        await DbContext.SaveChangesAsync();
    }

    [Test]
    public async Task Should_return_list_of_audits()
    {
        var response = await Client.GetAsync($"api/audits");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<ListAuditsResponse>();
        Assert.That(content, Is.Not.Null);

        var audit1 = content.Items.FirstOrDefault(x => x.AuditId.Equals(_audit1!.AuditId));
        Assert.That(audit1, Is.Not.Null);

        var audit2 = content.Items.FirstOrDefault(x => x.AuditId.Equals(_audit2!.AuditId));
        Assert.That(audit2, Is.Not.Null);

        var audit3 = content.Items.FirstOrDefault(x => x.AuditId.Equals(_audit3!.AuditId));
        Assert.That(audit3, Is.Not.Null);
    }

    [Test]
    public async Task Should_return_paginated_list_of_audits()
    {
        var response = await Client.GetAsync($"api/audits?pageSize=1&pageNumber=3");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<ListAuditsResponse>();

        Assert.That(content, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(content.Items, Has.Count.EqualTo(1));
            Assert.That(content.Metadata.CurrentPage, Is.EqualTo(3));
            Assert.That(content.Metadata.TotalPages, Is.EqualTo(3));
            Assert.That(content.Metadata.TotalCount, Is.EqualTo(3));
            Assert.That(content.Metadata.HasNextPage, Is.False);
            Assert.That(content.Metadata.HasPreviousPage, Is.True);
        });
    }

    [Test]
    public async Task Should_return_sorted_list_of_audits()
    {
        // Sort by author descending
        var response1 = await Client.GetAsync($"api/audits?orderBy=author desc");
        response1.EnsureSuccessStatusCode();

        var content1 = await response1.Content.ReadFromJsonAsync<ListAuditsResponse>();
        Assert.That(content1, Is.Not.Null);
        Assert.That(content1.Items.Select(x => x.Author), Is.Ordered.Descending);

        // Then sort by area ascending
        var response2 = await Client.GetAsync($"api/audits?orderBy=area asc");
        response2.EnsureSuccessStatusCode();

        var content2 = await response2.Content.ReadFromJsonAsync<ListAuditsResponse>();
        Assert.That(content2, Is.Not.Null);
        Assert.That(content2.Items.Select(x => x.Area), Is.Ordered.Ascending);
    }

    [Test]
    public async Task Should_throw_error_when_invalid_field_for_sorting()
    {
        var response = await Client.GetAsync($"api/audits?orderBy=score asc");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.That(content, Is.Not.Null);
        Assert.That(content.Detail, Is.EqualTo(new InvalidOrderByException("score asc").Message));
    }
}
