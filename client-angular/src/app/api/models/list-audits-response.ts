/* tslint:disable */
/* eslint-disable */
import { AuditListItemDto } from '../models/audit-list-item-dto';
import { PaginationMetadata } from '../models/pagination-metadata';
export interface ListAuditsResponse {
  items: Array<AuditListItemDto>;
  metadata: PaginationMetadata;
}
