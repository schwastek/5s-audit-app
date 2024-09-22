using Api.Contracts.Question.Requests;
using Core.MappingService;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Features.Question.List;

public sealed record ListQuestionsQuery : IRequest<ListQuestionsQueryResult>;

public sealed record ListQuestionsQueryResult
{
    public IReadOnlyList<Dto.QuestionDto> Questions { get; init; } = null!;
}

public class ListQuestionsQueryMapper : IMapper<ListQuestionsQueryResult, ListQuestionsResponse>
{
    private readonly IMappingService mapper;

    public ListQuestionsQueryMapper(IMappingService mapper)
    {
        this.mapper = mapper;
    }

    public ListQuestionsResponse Map(ListQuestionsQueryResult src)
    {
        var questions = src.Questions
            .Select(question => mapper.Map<Dto.QuestionDto, Api.Contracts.Question.Dto.QuestionDto>(question))
            .ToList()
            .AsReadOnly();

        return new ListQuestionsResponse()
        {
            Questions = questions
        };
    }
}
