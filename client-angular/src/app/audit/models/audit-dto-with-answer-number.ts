import { ApiAnswerDto, ApiAuditDto } from '../../api/models';

export interface AuditDtoWithAnswerNumber extends ApiAuditDto {
  answers: AnswerDtoWithAnswerNumber[];
}

export interface AnswerDtoWithAnswerNumber extends ApiAnswerDto {
  answerNumber: number;
}
