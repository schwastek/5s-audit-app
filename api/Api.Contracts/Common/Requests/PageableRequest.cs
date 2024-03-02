using System.Collections.Generic;

namespace Api.Contracts.Common.Requests
{
    public abstract class PageableRequest
    {
        /// <example>1</example>
        public int? PageNumber { get; set; }

        /// <example>5</example>
        public int? PageSize { get; set; }
    }

    public interface IPaginatedResult<T>
    {
        IReadOnlyCollection<T> Items { get; }
        IPaginationMetadata Metadata { get; }
    }

    public interface IPaginationMetadata
    {
        public int CurrentPage { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }
    }

    public class PaginationMetadata : IPaginationMetadata
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
