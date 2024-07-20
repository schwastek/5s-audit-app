/* tslint:disable */
/* eslint-disable */
import { ApiAuditListItemDto } from '../models/api-audit-list-item-dto';
import { ApiPaginationMetadata } from '../models/api-pagination-metadata';
export interface ApiListAuditsResponse {
  items: Array<ApiAuditListItemDto>;
  metadata: ApiPaginationMetadata;
}
