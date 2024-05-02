/* tslint:disable */
/* eslint-disable */
import { AnswerForCreationDto } from '../models/answer-for-creation-dto';
import { AuditActionForCreationDto } from '../models/audit-action-for-creation-dto';
export interface SaveAuditRequest {
  actions: Array<AuditActionForCreationDto>;
  answers: Array<AnswerForCreationDto>;
  area: string;
  auditId: string;
  author: string;
  endDate: string;
  startDate: string;
}
