/* tslint:disable */
/* eslint-disable */
import { AuditListItemDto } from '../models/audit-list-item-dto';
import { IPaginationMetadata } from '../models/i-pagination-metadata';
export interface ListAuditsResponse {
  items?: Array<AuditListItemDto> | null;
  metadata?: IPaginationMetadata;
}
