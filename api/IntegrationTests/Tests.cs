using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IntegrationTests;

[TestFixture]
internal sealed class Tests : BaseTestFixture
{
    [Test]
    public async Task Get_Audits_ReturnsSuccess()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "api/audits");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Act
        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        StringAssert.Contains("TestAuthor1", content);
        StringAssert.Contains("TestArea1", content);
    }

    [Test]
    public async Task Get_AuditById_ReturnsSuccess()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "api/audits/f285d889-27b2-4759-9d9f-0fffbdac982b");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Act
        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        StringAssert.Contains("f285d889-27b2-4759-9d9f-0fffbdac982b", content);
        StringAssert.Contains("answers", content);
        StringAssert.Contains("actions", content);
        StringAssert.Contains("\"answerText\":\"1\"", content);
    }

    [Test]
    public async Task Get_Questions_ReturnsSuccess()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "api/questions");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Act
        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        StringAssert.Contains("TestQuestion1", content);
        StringAssert.Contains("TestQuestion2", content);
    }
}
