using Api.Requests.AuditActions.Delete;
using Api.Requests.AuditActions.Save;
using Api.Requests.AuditActions.Update;
using Api.Requests.Audits.Get;
using Api.Requests.Audits.List;
using Api.Requests.Audits.Save;
using Api.Requests.Common;
using Api.Requests.Questions.List;
using Core.MappingService;
using Features.AuditActions.Delete;
using Features.AuditActions.Save;
using Features.AuditActions.Update;
using Features.Audits.Get;
using Features.Audits.List;
using Features.Audits.Save;
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
        services.AddSingleton<IMapper<Domain.Audit, Features.Audits.Dto.AuditDto>, Features.Audits.Dto.AuditDtoMapper>();
        services.AddSingleton<IMapper<Features.Audits.Dto.AuditListItemDto, Requests.Audits.Dto.AuditListItemDto>, Requests.Audits.Dto.AuditListItemDtoMapper>();

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
        services.AddSingleton<IMapper<Domain.Answer, Features.Answers.Dto.AnswerDto>, Features.Answers.Dto.AnswerDtoMapper>();
        services.AddSingleton<IMapper<Features.Answers.Dto.AnswerDto, Requests.Answers.Dto.AnswerDto>, Requests.Answers.Dto.AnswerDtoMapper>();
        services.AddSingleton<IMapper<Requests.Answers.Dto.AnswerForCreationDto, Features.Answers.Dto.AnswerForCreationDto>, Requests.Answers.Dto.AnswerForCreationDtoMapper>();
    }

    private static void AddAuditActionMappers(IServiceCollection services)
    {
        // DTO
        services.AddSingleton<IMapper<Domain.AuditAction, Features.AuditActions.Dto.AuditActionDto>, Features.AuditActions.Dto.AuditActionDtoMapper>();
        services.AddSingleton<IMapper<Features.AuditActions.Dto.AuditActionDto, Requests.AuditActions.Dto.AuditActionDto>, Requests.AuditActions.Dto.AuditActionDtoMapper>();
        services.AddSingleton<IMapper<Requests.AuditActions.Dto.AuditActionForCreationDto, Features.AuditActions.Dto.AuditActionForCreationDto>, Requests.AuditActions.Dto.AuditActionForCreationDtoMapper>();

        // CQRS
        services.AddSingleton<IMapper<SaveAuditActionRequest, SaveAuditActionCommand>, SaveAuditActionRequestMapper>();
        services.AddSingleton<IMapper<SaveAuditActionCommandResult, SaveAuditActionResponse>, SaveAuditActionCommandResultMapper>();

        services.AddSingleton<IMapper<DeleteAuditActionRequest, DeleteAuditActionCommand>, DeleteAuditActionRequestMapper>();
        services.AddSingleton<IMapper<UpdateAuditActionRequest, UpdateAuditActionCommand>, UpdateAuditActionRequestMapper>();
    }

    private static void AddQuestionMappers(IServiceCollection services)
    {
        // DTO
        services.AddSingleton<IMapper<Domain.Question, Features.Questions.Dto.QuestionDto>, Features.Questions.Dto.QuestionDtoMapper>();
        services.AddSingleton<IMapper<Features.Questions.Dto.QuestionDto, Requests.Questions.Dto.QuestionDto>, Requests.Questions.Dto.QuestionDtoMapper>();

        // CQRS
        services.AddSingleton<IMapper<ListQuestionsQueryResult, ListQuestionsResponse>, ListQuestionsQueryResultMapper>();
    }

    private static void AddPaginationMappers(IServiceCollection services)
    {
        services.AddSingleton<IMapper<Core.Pagination.PaginationMetadata, Requests.Common.PaginationMetadata>, PaginationMetadataMapper>();
    }
}
