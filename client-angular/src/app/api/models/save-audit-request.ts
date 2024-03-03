/* tslint:disable */
/* eslint-disable */
import { AnswerForCreationDto } from '../models/answer-for-creation-dto';
import { AuditActionForCreationDto } from '../models/audit-action-for-creation-dto';
export interface SaveAuditRequest {
  actions?: Array<AuditActionForCreationDto> | null;
  answers?: Array<AnswerForCreationDto> | null;
  area?: string | null;
  auditId?: string | null;
  author?: string | null;
  endDate?: string | null;
  startDate?: string | null;
}
