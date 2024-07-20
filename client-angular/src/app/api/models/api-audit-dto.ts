/* tslint:disable */
/* eslint-disable */
import { ApiAnswerDto } from '../models/api-answer-dto';
import { ApiAuditActionDto } from '../models/api-audit-action-dto';
export interface ApiAuditDto {
  actions: Array<ApiAuditActionDto>;
  answers: Array<ApiAnswerDto>;
  area: string;
  auditId: string;
  author: string;
  endDate: string;
  score: number;
  startDate: string;
}
