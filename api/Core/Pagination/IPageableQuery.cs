﻿namespace Core.Pagination;

public interface IPageableQuery
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}

public interface IOrderByQuery
{
    public string OrderBy { get; init; }
}