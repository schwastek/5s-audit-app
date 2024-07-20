import { ApiAnswerDto, ApiAuditDto, ApiAuditListItemDto } from '../../api/models';

export interface AuditDtoWithAnswerNumber extends ApiAuditDto {
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
