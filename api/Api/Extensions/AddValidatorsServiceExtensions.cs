using Api.Contracts.Audit.Requests;
using Api.Contracts.Identity.Requests;
using Api.Mappers.ValidatorService;
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
        // Universal service
        services.AddScoped<IValidatorService, ServiceLocatorValidatorService>();

        AddAuditValidators(services);
        AddIdentityValidators(services);
    }

    private static void AddAuditValidators(IServiceCollection services)
    {
        services.AddScoped<IAuditBusinessRules, AuditBusinessRules>();
        services.AddScoped<IValidator<SaveAuditCommand>, SaveAuditCommandValidator>();
        services.AddScoped<IValidator<GetAuditRequest>, GetAuditRequestValidator>();
    }

    private static void AddIdentityValidators(IServiceCollection services)
    {
        services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
    }
}
