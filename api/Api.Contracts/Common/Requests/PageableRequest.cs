using System.Collections.Generic;

namespace Api.Contracts.Common.Requests
{
    public interface IPageableRequest
    {
        /// <example>1</example>
        public int PageNumber { get; set; }

        /// <example>5</example>
        public int PageSize { get; set; }
    }

    public interface IPaginatedResult<T>
    {
        IReadOnlyCollection<T> Items { get; }
        PaginationMetadata Metadata { get; }
    }

    public class PaginationMetadata
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
