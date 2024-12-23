using Api.Contracts.Question.Requests;
using Domain;
using FluentAssertions;
using IntegrationTests.Helpers;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IntegrationTests.Questions;

[TestFixture]
internal class ListQuestionsTestFixture : BaseTestFixture
{
    private ICollection<Question>? _questions;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _questions = TestDataService.BuildQuestions(5);

        await DbContext.AddRangeAsync(_questions);
        await DbContext.SaveChangesAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (_questions is not null) DbContext.Questions.RemoveRange(_questions);

        await DbContext.SaveChangesAsync();
    }

    [Test]
    public async Task Should_return_list_of_questions()
    {
        var response = await Client.GetAsync($"api/questions");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<ListQuestionsResponse>();

        content.Should().NotBeNull();
        content!.Questions.Should().NotBeEmpty();
        content!.Questions.Should().HaveCount(_questions!.Count);
    }
}
