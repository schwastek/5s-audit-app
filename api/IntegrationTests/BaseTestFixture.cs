using IntegrationTests.AuthHandlers;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IntegrationTests;

internal class BaseTestFixture
{
    protected HttpClient Client { get; private set; }

    public BaseTestFixture()
    {
        var factory = new ApiWebApplicationFactory();
        Client = factory.CreateClient();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandlerConstants.AuthenticationScheme);
    }
}
