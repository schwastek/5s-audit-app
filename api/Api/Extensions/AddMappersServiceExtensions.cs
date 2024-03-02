using Api.Contracts.Audit.Requests;
using Api.Contracts.AuditAction.Requests;
using Api.Contracts.Question.Requests;
using Api.Exceptions;
using Api.Mappers.MappingService;
using Core.MappingService;
using Core.Pagination;
using Features.Answer.Dto;
using Features.Audit.Dto;
using Features.Audit.Get;
using Features.Audit.List;
using Features.Audit.Save;
using Features.AuditAction.Dto;
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
        AddOtherMappers(services);
    }

    private static void AddAuditMappers(IServiceCollection services)
    {
        // DTO
        services.AddSingleton<IMapper<Domain.Audit, Features.Audit.Dto.AuditDto>, AuditDtoMapper>();
        services.AddSingleton<IMapper<Features.Audit.Dto.AuditDto, Api.Contracts.Audit.Dto.AuditDto>, AuditDtoMapper>();

        services.AddSingleton<IMapper<Domain.Audit, Features.Audit.Dto.AuditListItemDto>, AuditListItemDtoMapper>();
        services.AddSingleton<IMapper<Features.Audit.Dto.AuditListItemDto, Api.Contracts.Audit.Dto.AuditListItemDto>, AuditListItemDtoMapper>();

        // CQRS
        services.AddSingleton<IMapper<GetAuditRequest, GetAuditQuery>, GetAuditQueryMapper>();
        services.AddSingleton<IMapper<GetAuditQueryResult, GetAuditResponse>, GetAuditQueryMapper>();

        services.AddSingleton<IMapper<ListAuditsRequest, ListAuditsQuery>, ListAuditsRequestMapper>();
        services.AddSingleton<IMapper<ListAuditsQueryResult, ListAuditsResponse>, ListAuditsRequestMapper>();

        services.AddSingleton<IMapper<SaveAuditRequest, SaveAuditCommand>, SaveAuditCommandMapper>();
        services.AddSingleton<IMapper<SaveAuditCommandResult, SaveAuditResponse>, SaveAuditCommandMapper>();
    }

    private static void AddAnswerMappers(IServiceCollection services)
    {
        // DTO
        services.AddSingleton<IMapper<Domain.Answer, AnswerDto>, AnswerDtoMapper>();
        services.AddSingleton<IMapper<AnswerDto, Api.Contracts.Answer.Dto.AnswerDto>, AnswerDtoMapper>();
        services.AddSingleton<IMapper<Api.Contracts.Answer.Dto.AnswerForCreationDto, AnswerForCreationDto>, AnswerForCreationDtoMapper>();
    }

    private static void AddAuditActionMappers(IServiceCollection services)
    {
        // DTO
        services.AddSingleton<IMapper<Domain.AuditAction, Features.AuditAction.Dto.AuditActionDto>, AuditActionDtoMapper>();
        services.AddSingleton<IMapper<Features.AuditAction.Dto.AuditActionDto, Api.Contracts.AuditAction.Dto.AuditActionDto>, AuditActionDtoMapper>();

        services.AddSingleton<IMapper<Api.Contracts.AuditAction.Dto.AuditActionForCreationDto,
            Features.AuditAction.Dto.AuditActionForCreationDto>, AuditActionForCreationDtoMapper>();

        // CQRS
        services.AddSingleton<IMapper<SaveAuditActionRequest, SaveAuditActionCommand>, SaveAuditActionCommandMapper>();
        services.AddSingleton<IMapper<SaveAuditActionCommandResult, SaveAuditActionResponse>, SaveAuditActionCommandMapper>();

        services.AddSingleton<IMapper<UpdateAuditActionRequest, UpdateAuditActionCommand>, UpdateAuditActionCommandMapper>();
    }

    private static void AddQuestionMappers(IServiceCollection services)
    {
        // DTO
        services.AddSingleton<IMapper<Domain.Question, Features.Question.Dto.QuestionDto>, QuestionMapper>();
        services.AddSingleton<IMapper<Features.Question.Dto.QuestionDto, Api.Contracts.Question.Dto.QuestionDto>, QuestionMapper>();

        // CQRS
        services.AddSingleton<IMapper<ListQuestionsQueryResult, ListQuestionsResponse>, ListQuestionsQueryMapper>();
    }

    private static void AddPaginationMappers(IServiceCollection services)
    {
        services.AddSingleton<IMapper<Core.Pagination.IPaginationMetadata, Api.Contracts.Common.Requests.IPaginationMetadata>, PaginationMapper>();
    }

    private static void AddOtherMappers(IServiceCollection services)
    {
        services.AddSingleton<IMapper<ErrorDetails, Contracts.Common.ErrorDetails>, ErrorDetailsMapper>();
    }
}
