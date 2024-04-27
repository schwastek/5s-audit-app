namespace Api.Contracts.Common.Requests
{
    public interface IOrderByRequest
    {
        /// <example>author asc, created desc</example>
        public string OrderBy { get; set; }
    }
}
