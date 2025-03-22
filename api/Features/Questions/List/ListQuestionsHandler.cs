using Data.DbContext;
using Domain;
using Features.Core.MappingService;
using Features.Questions.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Questions.List;

public sealed class ListQuestionsHandler : IRequestHandler<ListQuestionsQuery, ListQuestionsQueryResult>
{
    private readonly LeanAuditorContext _context;
    private readonly IMappingService _mapper;

    public ListQuestionsHandler(LeanAuditorContext context, IMappingService mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListQuestionsQueryResult> Handle(ListQuestionsQuery request, CancellationToken cancellationToken)
    {
        var questions = await _context.Questions.ToListAsync(cancellationToken);

        // Mapping
        var questionsList = questions
            .Select(_mapper.Map<Question, QuestionDto>)
            .ToList();

        var result = new ListQuestionsQueryResult()
        {
            Questions = questionsList
        };

        return result;
    }
}
