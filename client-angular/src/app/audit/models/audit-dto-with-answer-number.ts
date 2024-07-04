import { AnswerDto, AuditDto } from '../../api/models';

export interface AuditDtoWithAnswerNumber extends AuditDto {
  answers: AnswerDtoWithAnswerNumber[];
}

export interface AnswerDtoWithAnswerNumber extends AnswerDto {
  answerNumber: number;
}
