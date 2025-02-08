namespace Api.Requests.Common;

public interface IOrderByRequest
{
    /// <example>author asc, created desc</example>
    public string? OrderBy { get; set; }
}
