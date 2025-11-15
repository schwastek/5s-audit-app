using Data.Context;

namespace IntegrationTests.Context;

public class TestCurrentUserService : ICurrentUserService
{
    public string Username => AuditUsers.Test;
}
