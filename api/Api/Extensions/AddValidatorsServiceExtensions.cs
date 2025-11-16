using Api.Requests.Identity;
using Features.Accounts.BusinessRules;
using Features.AuditActions.BusinessRules;
using Features.AuditActions.Delete;
using Features.AuditActions.Save;
using Features.AuditActions.Update;
using Features.Audits.BusinessRules;
using Features.Audits.Get;
using Features.Audits.Save;
using Infrastructure.ValidatorService;
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

        services.AddScoped<AbstractValidator<GetAuditQuery>, GetAuditQueryValidator>();
        services.AddScoped<AbstractValidator<SaveAuditCommand>, SaveAuditCommandValidator>();
    }

    private static void AddAuditActionValidators(IServiceCollection services)
    {
        services.AddScoped<IAuditActionBusinessRules, AuditActionBusinessRules>();

        services.AddScoped<AbstractValidator<SaveAuditActionCommand>, SaveAuditActionCommandValidator>();
        services.AddScoped<AbstractValidator<DeleteAuditActionCommand>, DeleteAuditActionCommandValidator>();
        services.AddScoped<AbstractValidator<UpdateAuditActionCommand>, UpdateAuditActionCommandValidator>();
    }

    private static void AddIdentityValidators(IServiceCollection services)
    {
        services.AddScoped<IAccountBusinessRules, AccountBusinessRules>();

        services.AddScoped<AbstractValidator<RegisterRequest>, RegisterRequestValidator>();
        services.AddScoped<AbstractValidator<LoginRequest>, LoginRequestValidator>();
    }
}
