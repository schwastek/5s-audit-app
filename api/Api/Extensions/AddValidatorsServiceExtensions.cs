using Api.Requests.Identity;
using Core.ValidatorService;
using Features.Accounts.BusinessRules;
using Features.AuditActions.BusinessRules;
using Features.AuditActions.Delete;
using Features.AuditActions.Save;
using Features.AuditActions.Update;
using Features.Audits.BusinessRules;
using Features.Audits.Get;
using Features.Audits.Save;
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
        AddAuditActionValidators(services);
        AddIdentityValidators(services);
    }

    private static void AddAuditValidators(IServiceCollection services)
    {
        services.AddScoped<IAuditBusinessRules, AuditBusinessRules>();

        services.AddScoped<IValidator<GetAuditQuery>, GetAuditQueryValidator>();
        services.AddScoped<IValidator<SaveAuditCommand>, SaveAuditCommandValidator>();
    }

    private static void AddAuditActionValidators(IServiceCollection services)
    {
        services.AddScoped<IAuditActionBusinessRules, AuditActionBusinessRules>();

        services.AddScoped<IValidator<SaveAuditActionCommand>, SaveAuditActionCommandValidator>();
        services.AddScoped<IValidator<DeleteAuditActionCommand>, DeleteAuditActionCommandValidator>();
        services.AddScoped<IValidator<UpdateAuditActionCommand>, UpdateAuditActionCommandValidator>();
    }

    private static void AddIdentityValidators(IServiceCollection services)
    {
        services.AddScoped<IAccountBusinessRules, AccountBusinessRules>();

        services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
    }
}
