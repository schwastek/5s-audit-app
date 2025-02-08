using Api.Requests.Answers.Dto;
using Api.Requests.AuditActions.Delete;
using Api.Requests.AuditActions.Dto;
using Api.Requests.AuditActions.Save;
using Api.Requests.AuditActions.Update;
using Api.Requests.Audits.Dto;
using Api.Requests.Audits.Get;
using Api.Requests.Audits.List;
using Api.Requests.Audits.Save;
using Api.Requests.Common;
using Api.Requests.Questions.List;
using Core.MappingService;
using Features.Audit.Dto;
using Features.Audit.Get;
using Features.Audit.List;
using Features.Audit.Save;
using Features.AuditAction.Delete;
using Features.AuditAction.Save;
using Features.AuditAction.Update;
using Features.Question.Dto;
using Features.Question.List;
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
        services.AddSingleton<IMapper<Domain.Audit, Features.Audit.Dto.AuditDto>, AuditDtoMapper>();
        services.AddSingleton<IMapper<Features.Audit.Dto.AuditListItemDto, Requests.Audits.Dto.AuditListItemDto>, AuditListItemDtoMapper>();

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
        services.AddSingleton<IMapper<Domain.Answer, Features.Answer.Dto.AnswerDto>, Features.Answer.Dto.AnswerDtoMapper>();
        services.AddSingleton<IMapper<Features.Answer.Dto.AnswerDto, Requests.Answers.Dto.AnswerDto>, Requests.Answers.Dto.AnswerDtoMapper>();
        services.AddSingleton<IMapper<Requests.Answers.Dto.AnswerForCreationDto, Features.Answer.Dto.AnswerForCreationDto>, AnswerForCreationDtoMapper>();
    }

    private static void AddAuditActionMappers(IServiceCollection services)
    {
        // DTO
        services.AddSingleton<IMapper<Domain.AuditAction, Features.AuditAction.Dto.AuditActionDto>, Features.AuditAction.Dto.AuditActionDtoMapper>();
        services.AddSingleton<IMapper<Features.AuditAction.Dto.AuditActionDto, Requests.AuditActions.Dto.AuditActionDto>, Requests.AuditActions.Dto.AuditActionDtoMapper>();
        services.AddSingleton<IMapper<Requests.AuditActions.Dto.AuditActionForCreationDto, Features.AuditAction.Dto.AuditActionForCreationDto>, AuditActionForCreationDtoMapper>();

        // CQRS
        services.AddSingleton<IMapper<SaveAuditActionRequest, SaveAuditActionCommand>, SaveAuditActionRequestMapper>();
        services.AddSingleton<IMapper<SaveAuditActionCommandResult, SaveAuditActionResponse>, SaveAuditActionCommandResultMapper>();

        services.AddSingleton<IMapper<DeleteAuditActionRequest, DeleteAuditActionCommand>, DeleteAuditActionRequestMapper>();
        services.AddSingleton<IMapper<UpdateAuditActionRequest, UpdateAuditActionCommand>, UpdateAuditActionRequestMapper>();
    }

    private static void AddQuestionMappers(IServiceCollection services)
    {
        // DTO
        services.AddSingleton<IMapper<Domain.Question, Features.Question.Dto.QuestionDto>, QuestionDtoMapper>();
        services.AddSingleton<IMapper<Features.Question.Dto.QuestionDto, Requests.Questions.Dto.QuestionDto>, Requests.Questions.Dto.QuestionDtoMapper>();

        // CQRS
        services.AddSingleton<IMapper<ListQuestionsQueryResult, ListQuestionsResponse>, ListQuestionsQueryResultMapper>();
    }

    private static void AddPaginationMappers(IServiceCollection services)
    {
        services.AddSingleton<IMapper<Core.Pagination.PaginationMetadata, Requests.Common.PaginationMetadata>, PaginationMetadataMapper>();
    }
}
