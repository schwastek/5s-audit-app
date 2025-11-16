using Api.Requests.AuditActions.Delete;
using Api.Requests.AuditActions.Save;
using Api.Requests.AuditActions.Update;
using Api.Requests.Audits.Get;
using Api.Requests.Audits.List;
using Api.Requests.Audits.Save;
using Api.Requests.Common;
using Api.Requests.Questions.List;
using Features.Answers.Dto;
using Features.AuditActions.Delete;
using Features.AuditActions.Dto;
using Features.AuditActions.Save;
using Features.AuditActions.Update;
using Features.Audits.Dto;
using Features.Audits.Get;
using Features.Audits.List;
using Features.Audits.Save;
using Infrastructure.MappingService;
using Features.Questions.Dto;
using Features.Questions.List;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions;

public static class AddMappersServiceExtensions
{
    public static void AddMappers(this IServiceCollection services)
    {
        // Universal service
        services.AddSingleton<IMappingService, ServiceLocatorMappingService>();

        AddAuditMappers(services);
        AddAuditActionMappers(services);
        AddAnswerMappers(services);
        AddQuestionMappers(services);
        AddPaginationMappers(services);
    }

    private static void AddAuditMappers(IServiceCollection services)
    {
        // DTO
        services.AddSingleton<IMapper<Domain.Audit, AuditDto>, AuditDtoMapper>();
        services.AddSingleton<IMapper<AuditListItemDto, Requests.Audits.Dto.AuditListItemDto>, Requests.Audits.Dto.AuditListItemDtoMapper>();

        // CQRS
        services.AddSingleton<IMapper<GetAuditRequest, GetAuditQuery>, GetAuditRequestMapper>();
        services.AddSingleton<IMapper<GetAuditQueryResult, GetAuditResponse>, GetAuditQueryResultMapper>();

        services.AddSingleton<IMapper<ListAuditsRequest, ListAuditsQuery>, ListAuditsRequestMapper>();
        services.AddSingleton<IMapper<ListAuditsQueryResult, ListAuditsResponse>, ListAuditsQueryResultMapper>();

        services.AddSingleton<IMapper<SaveAuditRequest, SaveAuditCommand>, SaveAuditRequestMapper>();
        services.AddSingleton<IMapper<SaveAuditCommandResult, SaveAuditResponse>, SaveAuditCommandResultMapper>();
    }

    private static void AddAnswerMappers(IServiceCollection services)
    {
        // DTO
        services.AddSingleton<IMapper<Domain.Answer, AnswerDto>, AnswerDtoMapper>();
        services.AddSingleton<IMapper<AnswerDto, Requests.Answers.Dto.AnswerDto>, Requests.Answers.Dto.AnswerDtoMapper>();
        services.AddSingleton<IMapper<Requests.Answers.Dto.AnswerForCreationDto, AnswerForCreationDto>, Requests.Answers.Dto.AnswerForCreationDtoMapper>();
    }

    private static void AddAuditActionMappers(IServiceCollection services)
    {
        // DTO
        services.AddSingleton<IMapper<Domain.AuditAction, AuditActionDto>, AuditActionDtoMapper>();
        services.AddSingleton<IMapper<AuditActionDto, Requests.AuditActions.Dto.AuditActionDto>, Requests.AuditActions.Dto.AuditActionDtoMapper>();
        services.AddSingleton<IMapper<Requests.AuditActions.Dto.AuditActionForCreationDto, AuditActionForCreationDto>, Requests.AuditActions.Dto.AuditActionForCreationDtoMapper>();

        // CQRS
        services.AddSingleton<IMapper<SaveAuditActionRequest, SaveAuditActionCommand>, SaveAuditActionRequestMapper>();
        services.AddSingleton<IMapper<SaveAuditActionCommandResult, SaveAuditActionResponse>, SaveAuditActionCommandResultMapper>();

        services.AddSingleton<IMapper<DeleteAuditActionRequest, DeleteAuditActionCommand>, DeleteAuditActionRequestMapper>();
        services.AddSingleton<IMapper<UpdateAuditActionRequest, UpdateAuditActionCommand>, UpdateAuditActionRequestMapper>();
    }

    private static void AddQuestionMappers(IServiceCollection services)
    {
        // DTO
        services.AddSingleton<IMapper<Domain.Question, QuestionDto>, QuestionDtoMapper>();
        services.AddSingleton<IMapper<QuestionDto, Requests.Questions.Dto.QuestionDto>, Requests.Questions.Dto.QuestionDtoMapper>();

        // CQRS
        services.AddSingleton<IMapper<ListQuestionsQueryResult, ListQuestionsResponse>, ListQuestionsQueryResultMapper>();
    }

    private static void AddPaginationMappers(IServiceCollection services)
    {
        services.AddSingleton<IMapper<Infrastructure.Pagination.PaginationMetadata, Requests.Common.PaginationMetadata>, PaginationMetadataMapper>();
    }
}
