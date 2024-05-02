namespace Api.Contracts.Common
{
    public class ErrorDetails
    {
        /// <example>404</example>
        public int StatusCode { get; set; }

        /// <example>Audit not found</example>
        public string Message { get; set; } = null!;
    }
}
