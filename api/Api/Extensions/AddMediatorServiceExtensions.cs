using Features.AuditActions.Delete;
using Features.AuditActions.Save;
using Features.AuditActions.Update;
using Features.Audits.Get;
using Features.Audits.List;
using Features.Audits.Save;
using Infrastructure.MediatorService;
using Infrastructure.MediatorService.Extensions;
using Infrastructure.MediatorService.Pipelines;
using Features.Questions.List;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions;

public static class AddMediatorServiceExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatorRequiredServices();
        AddPipelines(services);
        AddAuditHandlers(services);
        AddAuditActionHandlers(services);
        AddQuestionHandlers(services);

        return services;
    }

    private static void AddPipelines(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
    }

    private static void AddAuditHandlers(this IServiceCollection services)
    {
        services.AddTransient<IRequestHandler<GetAuditQuery, GetAuditQueryResult>, GetAuditQueryHandler>();
        services.AddTransient<IRequestHandler<ListAuditsQuery, ListAuditsQueryResult>, ListAuditsQueryHandler>();
        services.AddTransient<IRequestHandler<SaveAuditCommand, SaveAuditCommandResult>, SaveAuditCommandHandler>();
    }

    private static void AddAuditActionHandlers(this IServiceCollection services)
    {
        services.AddTransient<IRequestHandler<DeleteAuditActionCommand, Unit>, DeleteAuditActionHandler>();
        services.AddTransient<IRequestHandler<SaveAuditActionCommand, SaveAuditActionCommandResult>, SaveAuditActionCommandHandler>();
        services.AddTransient<IRequestHandler<UpdateAuditActionCommand, Unit>, UpdateAuditActionHandler>();
    }

    private static void AddQuestionHandlers(this IServiceCollection services)
    {
        services.AddTransient<IRequestHandler<ListQuestionsQuery, ListQuestionsQueryResult>, ListQuestionsHandler>();
    }
}
