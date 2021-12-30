using Api.Helpers;
using Microsoft.AspNetCore.Routing;
using System;

namespace Api.ResourceParameters
{
    public class AuditsUrlQueryParameters
    {
        // Don't let the user pass page number -1 in the URL params
        private const int minPageNumber = 1;
        private const int maxPageSize = 20;

        private int _pageNumber = 1;
        private int _pageSize = 1;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value < minPageNumber) ? minPageNumber : value; 
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public string OrderBy { get; set; }
    }
}
