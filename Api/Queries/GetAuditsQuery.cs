using Api.Helpers;
using Api.Models;
using Api.ResourceParameters;
using MediatR;
using System.Collections.Generic;

namespace Api.Queries;

public sealed record GetAuditsQuery(AuditsUrlQueryParameters QueryParameters) 
    : IRequest<(IEnumerable<AuditListDto> audits, MetaData metaData)>;
