using Api.IntegrationTests.AuthHandlers;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace Api.IntegrationTests
{
    public class Tests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public Tests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandlerConstants.AuthenticationScheme);
        }

        [Fact]
        public async Task Get_Audits_ReturnsSuccess()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/audits");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("TestAuthor1", content);
            Assert.Contains("TestArea1", content);
        }

        [Fact]
        public async Task Get_AuditById_ReturnsSuccess()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/audits/f285d889-27b2-4759-9d9f-0fffbdac982b");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("f285d889-27b2-4759-9d9f-0fffbdac982b", content);
            Assert.Contains("answers", content);
            Assert.Contains("actions", content);
            Assert.Contains("\"answerText\":\"1\"", content);
        }

        [Fact]
        public async Task Get_Questions_ReturnsSuccess()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "api/questions");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("TestQuestion1", content);
            Assert.Contains("TestQuestion2", content);
        }
    }
}
