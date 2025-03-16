using Core.MappingService;
using Data.DbContext;
using Domain;
using Features.Questions.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Questions.List;

public sealed class ListQuestionsHandler : IRequestHandler<ListQuestionsQuery, ListQuestionsQueryResult>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;

    public ListQuestionsHandler(LeanAuditorContext context, IMappingService mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<ListQuestionsQueryResult> Handle(ListQuestionsQuery request, CancellationToken cancellationToken)
    {
        var questions = await context.Questions.ToListAsync(cancellationToken);

        // Mapping
        var questionsList = questions
            .Select(question => mapper.Map<Question, QuestionDto>(question))
            .ToList();

        var result = new ListQuestionsQueryResult()
        {
            Questions = questionsList
        };

        return result;
    }
}
