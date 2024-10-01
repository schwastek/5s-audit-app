using Api.Contracts.Audit.Requests;
using Api.Contracts.AuditAction.Requests;
using Api.Contracts.Identity.Requests;
using Api.Contracts.Internal.Audit;
using Core.Identity;
using Core.ValidatorService;
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
        // Universal service.
        services.AddScoped<IValidatorService, ServiceLocatorValidatorService>();

        AddAuditValidators(services);
        AddIdentityValidators(services);
    }

    private static void AddAuditValidators(IServiceCollection services)
    {
        services.AddScoped<IAuditBusinessRules, AuditBusinessRules>();
        services.AddScoped<IValidator<SaveAuditCommand>, SaveAuditCommandValidator>();

        services.AddScoped<IValidator<GetAuditRequest>, GetAuditRequestValidator>();
        services.AddScoped<IValidator<GetAuditQuery>, GetAuditQueryValidator>();

        services.AddScoped<IValidator<SaveAuditRequest>, SaveAuditRequestValidator>();
        services.AddScoped<IValidator<SaveAuditActionRequest>, SaveAuditActionRequestValidator>();
    }

    private static void AddIdentityValidators(IServiceCollection services)
    {
        services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
    }
}
