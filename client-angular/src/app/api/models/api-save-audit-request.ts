/* tslint:disable */
/* eslint-disable */
import { ApiAnswerForCreationDto } from '../models/api-answer-for-creation-dto';
import { ApiAuditActionForCreationDto } from '../models/api-audit-action-for-creation-dto';
export interface ApiSaveAuditRequest {
  actions: Array<ApiAuditActionForCreationDto>;
  answers: Array<ApiAnswerForCreationDto>;
  area: string;
  auditId: string;
  author: string;
  endDate: string;
  startDate: string;
}
