using Core.MappingService;
using Features.Questions.List;
using System.Linq;

namespace Api.Requests.Questions.List;

public sealed class ListQuestionsQueryResultMapper : IMapper<ListQuestionsQueryResult, ListQuestionsResponse>
{
    private readonly IMappingService _mapper;

    public ListQuestionsQueryResultMapper(IMappingService mapper)
    {
        _mapper = mapper;
    }

    public ListQuestionsResponse Map(ListQuestionsQueryResult src)
    {
        var questions = src.Questions
            .Select(_mapper.Map<Features.Questions.Dto.QuestionDto, Requests.Questions.Dto.QuestionDto>)
            .ToList();

        return new ListQuestionsResponse()
        {
            Questions = questions
        };
    }
}
