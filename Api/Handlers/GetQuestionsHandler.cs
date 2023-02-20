using Api.Data;
using Api.Core.Domain;
using Api.Mappers;
using Api.Models;
using Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Handlers;

public sealed class GetQuestionsHandler : IRequestHandler<GetQuestionsQuery, List<QuestionDto>>
{
    private readonly LeanAuditorContext context;
    private readonly IMappingService mapper;

    public GetQuestionsHandler(LeanAuditorContext context, IMappingService mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<QuestionDto>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        List<Question> questions = await context.Questions.ToListAsync();

        // Mapping
        List<QuestionDto> questionsDto = questions
            .Select(q => mapper.Map<Question, QuestionDto>(q))
            .ToList();

        return questionsDto;
    }
}
