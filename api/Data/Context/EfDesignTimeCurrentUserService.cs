namespace Data.Context;

/// <summary>
/// Design-time user service implementation for Entity Framework tooling.
/// Used when running migrations, where no HttpContext or real user exists.
/// This service provides a fixed identity so audit fields can be populated safely.
/// </summary>
public sealed class EfDesignTimeCurrentUserService : ICurrentUserService
{
    public string Username => AuditUsers.System;
}
