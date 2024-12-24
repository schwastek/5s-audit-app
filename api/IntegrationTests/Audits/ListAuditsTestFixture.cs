using Api.Contracts.Audit.Requests;
using Core.OrderByService;
using Domain;
using FluentAssertions;
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
        // TODO: Add extension method for removing.
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

        content.Should().NotBeNull();
        content!.Items.FirstOrDefault(x => x.AuditId.Equals(_audit1!.AuditId)).Should().NotBeNull();
        content!.Items.FirstOrDefault(x => x.AuditId.Equals(_audit2!.AuditId)).Should().NotBeNull();
        content!.Items.FirstOrDefault(x => x.AuditId.Equals(_audit3!.AuditId)).Should().NotBeNull();
    }

    [Test]
    public async Task Should_return_paginated_list_of_audits()
    {
        var response = await Client.GetAsync($"api/audits?pageSize=1&pageNumber=3");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<ListAuditsResponse>();

        content.Should().NotBeNull();
        content!.Items.Should().HaveCount(1);
        content!.Metadata.CurrentPage.Should().Be(3);
        content!.Metadata.TotalPages.Should().Be(3);
        content!.Metadata.TotalCount.Should().Be(3);
        content!.Metadata.HasNextPage.Should().BeFalse();
        content!.Metadata.HasPreviousPage.Should().BeTrue();
    }

    [Test]
    public async Task Should_return_sorted_list_of_audits()
    {
        // Sort by author descending
        var response1 = await Client.GetAsync($"api/audits?orderBy=author desc");
        response1.EnsureSuccessStatusCode();
        var content1 = await response1.Content.ReadFromJsonAsync<ListAuditsResponse>();

        content1.Should().NotBeNull();
        content1!.Items.Should().BeInDescendingOrder(x => x.Author);

        // Then sort by area ascending
        var response2 = await Client.GetAsync($"api/audits?orderBy=area asc");
        response2.EnsureSuccessStatusCode();
        var content2 = await response2.Content.ReadFromJsonAsync<ListAuditsResponse>();

        content2.Should().NotBeNull();
        content2!.Items.Should().BeInAscendingOrder(x => x.Area);
    }

    [Test]
    public async Task Should_throw_error_when_invalid_field_for_sorting()
    {
        var response = await Client.GetAsync($"api/audits?orderBy=score asc");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        content.Should().NotBeNull();
        content!.Detail.Should().BeEquivalentTo(new InvalidOrderByException("score asc").Message);
    }
}
