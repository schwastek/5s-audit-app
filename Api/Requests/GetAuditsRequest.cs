using Api.Common;

namespace Api.Requests;

public class GetAuditsRequest : PageableRequest
{
    /// <example>author asc</example>
    public string OrderBy { get; set; } = "author asc";
}
