/* tslint:disable */
/* eslint-disable */
import { AnswerDto } from '../models/answer-dto';
import { AuditActionDto } from '../models/audit-action-dto';
export interface AuditDto {
  actions: Array<AuditActionDto>;
  answers: Array<AnswerDto>;
  area: string;
  auditId: string;
  author: string;
  endDate: string;
  score: number;
  startDate: string;
}
