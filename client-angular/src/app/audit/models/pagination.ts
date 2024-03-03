export class PaginationMetadata
{
    currentPage: number;
    totalPages: number;
    pageSize: number;
    totalCount: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;

    constructor(
        currentPage: number = 1,
        totalPages: number = 1,
        pageSize: number = 1,
        totalCount: number = 1,
        hasPreviousPage: boolean = false,
        hasNextPage: boolean = false
    ) {
        this.currentPage = currentPage,
        this.totalPages = totalPages,
        this.pageSize = pageSize,
        this.totalCount = totalCount,
        this.hasPreviousPage = hasPreviousPage,
        this.hasNextPage = hasNextPage
    }
}

export class PaginatedResult<T> {
    items: T[];
    metadata: PaginationMetadata;

    constructor(
        items: T[] = [],
        metadata: PaginationMetadata = new PaginationMetadata()
    ) {
        this.items = items;
        this.metadata = metadata;
    }
}
