import { Component, inject, OnInit, signal } from '@angular/core';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuditService } from '../audit.service';
import { Area } from '../models/area';
import { firstValueFrom } from 'rxjs';
import { RatingComponent } from '../../shared/components/rating/rating.component';
import { ApiQuestionDto } from '../../api/models';
import { v4 as uuidv4 } from 'uuid';
import { Router } from '@angular/router';
import { AuditActionComponent, SaveOrUpdateAuditAction } from '../audit-action/audit-action.component';
import { LoadingButtonDirective } from '../../shared/components/loading-button/loading-button.directive';

// Typed form models
interface AnswerGroup {
  questionId: FormControl<string>;
  rating: FormControl<number>;
}

interface AuditForm {
  area: FormControl<string>;
  answers: FormArray<FormGroup<AnswerGroup>>;
}

@Component({
  selector: 'app-audit-new',
  templateUrl: './audit-new.component.html',
  imports: [
    ReactiveFormsModule,
    RatingComponent,
    AuditActionComponent,
    LoadingButtonDirective
  ]
})
export class AuditNewComponent implements OnInit {
  private auditService = inject(AuditService);
  private router = inject(Router);

  readonly auditId = uuidv4();
  private readonly startDate = new Date().toISOString();

  // Content
  areas= signal<Area[] | null>(null);
  auditActions = signal<SaveOrUpdateAuditAction[]>([]);
  questions: ApiQuestionDto[] | null = null;

  // Form
  area = new FormControl<string>('assembly', { validators: [Validators.required], nonNullable: true });
  answers = new FormArray<FormGroup<AnswerGroup>>([]);
  form = new FormGroup<AuditForm>({
    area: this.area,
    answers: this.answers
  });

  // Form status
  isQuestionsReady = signal(false);
  isFormSaving = signal(false);

  // Configuration
  private readonly defaultAnswer = 3;

  ngOnInit() {
    this.getAreas();
    this.getQuestions()
      .then(() => this.addRatings())
      .then(() => this.isQuestionsReady.set(true));
  }

  async onSave() {
    if (this.form.invalid) return;

    this.isFormSaving.set(true);

    // Generate IDs here to ensure each submission has unique IDs.
    // If saving fails and user resubmits, new IDs will prevent errors from duplicate IDs in DB.
    const answers = this.answers.value.map((a) => {
      return {
        answerId: uuidv4(),
        // Disabled controls are excluded from a FormGroup/FormArray's .value.
        // Therefore, .value is typed as Partial<...>, meaning fields may be undefined.
        questionId: a.questionId!,
        answerText: a.rating!.toString(),
        answerType: 'number'
      }
    });

    const audit = {
      auditId: this.auditId,
      author: 'John',
      area: this.area.value,
      startDate: this.startDate,
      endDate: new Date().toISOString(),
      answers: answers,
      actions: this.auditActions()
    };

    try {
      const response = await firstValueFrom(this.auditService.saveAudit(audit));
      this.router.navigate(['audits', response.auditId]);
    } finally {
      this.isFormSaving.set(false);
    }
  }

  private async getAreas() {
    const areas = await firstValueFrom(this.auditService.getAreas());
    this.areas.set(areas);
  }

  private async getQuestions() {
    this.questions = await firstValueFrom(this.auditService.getQuestions());
  }

  private addRatings() {
    this.answers.clear();

    this.questions!.forEach((q) => {
      const questionIdControl = new FormControl<string>(q.questionId, { nonNullable: true });
      const ratingControl = new FormControl<number>(this.defaultAnswer, { nonNullable: true });
      const answerGroup = new FormGroup({
        questionId: questionIdControl,
        rating: ratingControl
      });

      this.answers.push(answerGroup);
    });
  }
}
