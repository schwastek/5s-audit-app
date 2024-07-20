import { ApiPaginationMetadata } from '../../api/models';
import { Nullable } from '../../shared/ts-helpers/ts-helpers';

export class PaginationMetadata implements ApiPaginationMetadata
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
    this.currentPage = currentPage;
    this.totalPages = totalPages;
    this.pageSize = pageSize;
    this.totalCount = totalCount;
    this.hasPreviousPage = hasPreviousPage;
    this.hasNextPage = hasNextPage;
  }
}

export class PaginatedResult<T> {
  items: T[];
  metadata: PaginationMetadata;

  constructor(
    items: Nullable<T[]> = null,
    metadata: Nullable<ApiPaginationMetadata> = null
  ) {
    this.items = items ?? [];
    this.metadata = new PaginationMetadata(
      metadata?.currentPage, metadata?.totalPages, metadata?.pageSize,
      metadata?.totalCount, metadata?.hasPreviousPage, metadata?.hasNextPage
    );
  }
}
