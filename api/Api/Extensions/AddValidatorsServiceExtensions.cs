using Api.Contracts.Audit.Requests;
using Api.Contracts.Identity.Requests;
using Api.Contracts.Internal.Account;
using Api.Contracts.Internal.Audit;
using Core.ValidatorService;
using Features.Account.BusinessRules;
using Features.Audit.BusinessRules;
using Features.Audit.Get;
using Features.AuditAction.BusinessRules;
using Features.AuditAction.Delete;
using Features.AuditAction.Save;
using Features.AuditAction.Update;
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

        services.AddScoped<IValidator<GetAuditRequest>, GetAuditRequestValidator>();
        services.AddScoped<IValidator<GetAuditQuery>, GetAuditQueryValidator>();

        services.AddScoped<IValidator<SaveAuditRequest>, SaveAuditRequestValidator>();
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
