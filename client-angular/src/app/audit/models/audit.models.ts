import { ApiAnswerDto, ApiAuditListItemDto, ApiGetAuditResponse } from '../../api/models';

export interface AuditDtoWithAnswerNumber extends ApiGetAuditResponse {
  answers: AnswerDtoWithAnswerNumber[];
}

export interface AnswerDtoWithAnswerNumber extends ApiAnswerDto {
  answerNumber: number;
}

export interface AuditListItemDto extends ApiAuditListItemDto {
  auditIdShort: string;
  scorePercentage: string;
  startDateFormatted: string;
}
