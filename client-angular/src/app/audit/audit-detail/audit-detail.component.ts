import { Component, Input, OnInit } from '@angular/core';
import { AuditService } from '../audit.service';
import { lastValueFrom } from 'rxjs';
import { NgFor, NgIf } from '@angular/common';
import { RatingComponent } from '../../shared/components/rating/rating.component';
import { FormsModule } from '@angular/forms';
import { Nullable } from '../../shared/utilities/ts-helpers';
import { AnswerDtoWithAnswerNumber, AuditDtoWithAnswerNumber } from '../models/audit-dto-with-answer-number';
import { ApiAnswerDto } from '../../api/models';

@Component({
  selector: 'app-audit-detail',
  templateUrl: './audit-detail.component.html',
  styleUrls: ['./audit-detail.component.scss'],
  standalone: true,
  imports: [NgIf, NgFor, FormsModule, RatingComponent]
})
export class AuditDetailComponent implements OnInit {
  @Input() id?: string;
  audit?: Nullable<AuditDtoWithAnswerNumber>;

  constructor(
    private auditService: AuditService
  ) {}

  async ngOnInit() {
    this.audit = await this.getAudit(this.id);
  }

  answerTrackBy(_: number, answer: AnswerDtoWithAnswerNumber) {
    return answer.answerId;
  }

  async getAudit(id: Nullable<string>) {
    const audit = await lastValueFrom(this.auditService.getAudit(id));

    // Get answers as numbers instead of strings: '3' -> 3.
    const answers = this.convertStringsToNumbers(audit?.answers) ?? [];

    const transformedAudit = audit as AuditDtoWithAnswerNumber;
    transformedAudit.answers = answers;

    return transformedAudit;
  }

  private convertStringsToNumbers(answers: Nullable<ApiAnswerDto[]>): AnswerDtoWithAnswerNumber[] | undefined {
    const result = answers?.map((answer) => {
      const transformedAnswer = answer as AnswerDtoWithAnswerNumber;
      const number = Number(answer.answerText);
      const answerNumber = isNaN(number) ? 0 : number;
      transformedAnswer.answerNumber = answerNumber;

      return transformedAnswer;
    });

    return result;
  }
}
