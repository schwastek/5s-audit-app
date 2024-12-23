using Data.DbContext;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace IntegrationTests;

internal class BaseTestFixture
{
    private IServiceScope? _scope;
    private LeanAuditorContext? _dbContext;

    protected ApiWebApplicationFactory Factory { get; private set; } = null!;
    protected HttpClient Client { get; private set; } = null!;
    protected IServiceScope Scope { get => _scope ?? throw new InvalidOperationException($"The field '{nameof(_scope)}' has not been initialized."); }
    protected LeanAuditorContext DbContext { get => _dbContext ?? throw new InvalidOperationException($"The field '{nameof(_dbContext)}' has not been initialized."); }

    [OneTimeSetUp]
    public void BaseOneTimeSetUp()
    {
        Factory = GlobalTestFixture.Factory;
        Client = GlobalTestFixture.Client;

        _scope = Factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<LeanAuditorContext>();
    }

    [OneTimeTearDown]
    public void BaseOneTimeTearDown()
    {
        _dbContext?.Dispose();
        _scope?.Dispose();
    }
}
