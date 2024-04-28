using Api.Contracts.Identity.Requests;
using Core.Identity;
using Features.Audit.BusinessRules;
using Features.Audit.Get;
using Features.Audit.Save;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions;

public static class AddValidatorsServiceExtensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        AddAuditValidators(services);
        AddIdentityValidators(services);
    }

    private static void AddAuditValidators(IServiceCollection services)
    {
        services.AddScoped<IAuditBusinessRules, AuditBusinessRules>();
        services.AddScoped<IValidator<SaveAuditCommand>, SaveAuditCommandValidator>();
    }

    private static void AddIdentityValidators(IServiceCollection services)
    {
        services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
    }
}
