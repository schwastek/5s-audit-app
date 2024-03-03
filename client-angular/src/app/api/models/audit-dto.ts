/* tslint:disable */
/* eslint-disable */
import { AnswerDto } from '../models/answer-dto';
import { AuditActionDto } from '../models/audit-action-dto';
export interface AuditDto {
  actions?: Array<AuditActionDto> | null;
  answers?: Array<AnswerDto> | null;
  area?: string | null;
  auditId?: string;
  author?: string | null;
  endDate?: string;
  score?: number;
  startDate?: string;
}
