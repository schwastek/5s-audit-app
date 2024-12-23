using IntegrationTests.AuthHandlers;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IntegrationTests;

/// <summary>
/// This class that contains the one-time setup or teardown methods for all the test fixtures
/// in a given namespace including nested namespaces below, within an assembly.
/// </summary>
[SetUpFixture]
public class GlobalTestFixture
{
    private static ApiWebApplicationFactory? _factory;
    private static HttpClient? _client;

    public static ApiWebApplicationFactory Factory { get => _factory ?? throw new InvalidOperationException($"The field '{nameof(_factory)}' has not been initialized."); }
    public static HttpClient Client { get => _client ?? throw new InvalidOperationException($"The field '{nameof(_client)}' has not been initialized."); }

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        // Use a single instance of WebApplicationFactory for all tests.
        _factory = new ApiWebApplicationFactory();
        _client = _factory.CreateClient();

        // TODO: Configure headers per HTTP request.
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandlerConstants.AuthenticationScheme);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}
