using Api.Models;
using MediatR;
using System.Collections.Generic;

namespace Api.Queries
{
    public sealed record GetQuestionsQuery() : IRequest<List<QuestionDto>>;
}
